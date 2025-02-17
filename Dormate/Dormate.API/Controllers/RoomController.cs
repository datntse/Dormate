using AutoMapper;
using CMMS.API.Helpers;
using Dormate.API.Services;
using Dormate.Core.Constrants;
using Dormate.Core.Entities;
using Dormate.Core.Models;
using Dormate.Infrastructure.Data;
using Dormate.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Dormate.API.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IRoomService _roomService;
        private ICurrentUserService _currentUserService;
        private IRoomImageService _roomImageService;
        private IFirebaseService _firebaseService;
        private IMapper _mapper;
        private IUserService _userSerivce;
        private IServiceScopeFactory _serviceScopeFactory;

        public RoomController(IRoomService roomService, ICurrentUserService currentUserService,
            IRoomImageService roomImageService, IFirebaseService firebaseService, IMapper mapper,
            IUserService userService, IServiceScopeFactory serviceScopeFactory)
        {
            _roomService = roomService;
            _currentUserService = currentUserService;
            _roomImageService = roomImageService;
            _firebaseService = firebaseService;
            _mapper = mapper;
            _userSerivce = userService;
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] RoomFilterModel filterModel)
        {
            var filteredList = _roomService.Get(_ =>
                 (string.IsNullOrEmpty(filterModel.Id) || _.Id.Equals(filterModel.Id)) &&
                 (string.IsNullOrEmpty(filterModel.Name) || _.Name.Contains(filterModel.Name)) &&
                 (string.IsNullOrEmpty(filterModel.Province) || _.Province.Equals(filterModel.Province)) &&
                 (string.IsNullOrEmpty(filterModel.Ward) || _.Ward.Equals(filterModel.Ward)) &&
                 (string.IsNullOrEmpty(filterModel.District) || _.District.Equals(filterModel.District)) &&
                 (string.IsNullOrEmpty(filterModel.Address) || _.District.Contains(filterModel.Address)) &&
                 (filterModel.Status == null || _.Status.Equals(filterModel.Status)) &&
                 (filterModel.RoomType == null || _.RoomType.Equals(filterModel.RoomType)) &&
                 (filterModel.FromPrice == null || _.Price >= filterModel.FromPrice) &&
                 (filterModel.ToPrice == null || _.Price <= filterModel.ToPrice),
                     _ => _.Owner, _ => _.RoomImages);

            var total = filteredList.Count();
            var filterListPaged = filteredList.ToPageList(filterModel.defaultSearch.currentPage, filterModel.defaultSearch.perPage)
                .Sort(filterModel.defaultSearch.sortBy, filterModel.defaultSearch.isAscending);
            var result = _mapper.Map<List<RoomVM>>(filterListPaged);

            foreach (var roomVM in result)
            {
                var listRoomImage = _roomImageService.Get(_ => _.RoomId.Equals(roomVM.Id)).ToList();
                var subImageObject = _mapper.Map<List<SubImage>>(listRoomImage);
                roomVM.SubPictureUrl = subImageObject;
                roomVM.Owner = (await _userSerivce.FindByUserName(roomVM.Owner)).FullName;
            }
            return Ok(new Response
            {
                status = "Success",
                message = "Success",
                data = new
                {
                    rooms = result,
                    pagination = new
                    {
                        total,
                        perPage = filterModel.defaultSearch.perPage,
                        currentPage = filterModel.defaultSearch.currentPage,
                    }
                }
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var room = _roomService.Get(_ => _.Id.Equals(id)).FirstOrDefault();
            if (room == null)
            {
                return BadRequest(new Response
                {
                    status = "Failed",
                    message = "Room was not existed!",
                });
            }

            var roomVM = _mapper.Map<RoomVM>(room);
            var listRoomImage = _roomImageService.Get(_ => _.RoomId.Equals(roomVM.Id)).ToList();
            var subImageObject = _mapper.Map<List<SubImage>>(listRoomImage);
            roomVM.SubPictureUrl = subImageObject;
            roomVM.Owner = (await _userSerivce.FindByUserName(roomVM.Owner)).FullName;
            return Ok(new Response
            {
                status = "Success",
                message = "Success",
                data = roomVM

            });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoom(RoomUM roomUM)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _efTranscationScope = scope.ServiceProvider.GetRequiredService<IEfTransaction>();
                var room = _roomService.Get(_ => _.Id.Equals(roomUM.Id)).FirstOrDefault();
                if (room == null)
                {
                    return BadRequest(new Response
                    {
                        status = "Failed",
                        message = "Room was not existed!",
                    });
                }
      
                room.Name = roomUM.Name;
                room.Description = roomUM.Description;

                room.District = roomUM.District;
                room.Province = roomUM.Province;
                room.Ward = roomUM.Ward;
                room.Address = roomUM.Address;

                room.Area = roomUM.Area;
                room.Price = roomUM.Price;
                room.MaximunSlot = roomUM.MaximunSlot;
                room.CurrentSlot = roomUM.CurrentSlot;

                room.RoomType = roomUM.RoomType;


                _roomService.Update(room);
                var isUpdated = await _roomService.SaveChangeAsync();
                //foreach (var subImageUrl in roomUM.SubPictureUrl)
                //{
                //    var subImageEntity = _roomImageService.Get(_ => _.ImageUrl.Equals(subImageUrl)).FirstOrDefault();
                //    if (subImageEntity == null)
                //    {
                //        await _efTranscationScope.RollbackAsync();
                //        return BadRequest(new Response
                //        {
                //            status = "Failed",
                //            message = "Room was not existed!",
                //        });
                //    }
                //}
                await _efTranscationScope.CommitAsync();

                return Ok(new Response
                {
                    status = "Success",
                    message = "Update room sucessfully",
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewRoomAsync([FromForm] RoomCM roomCM)
        {
            var userId = _currentUserService.GetUserId();

            if (roomCM.MainPicture == null || roomCM.MainPicture.Length == 0)
            {
                return BadRequest(new Response
                {
                    status = "Failed",
                    message = "File not provided!",
                });
            }

            var roomId = _roomService.GenerateRoomCode();

            var addRoom = _mapper.Map<Room>(roomCM);
            addRoom.Id = roomId;
            addRoom.OwnerId = userId;
            addRoom.CreatedAt = TimeConverter.GetVietNamTime();

            var mainImageUrl = await _firebaseService.UploadFileAsync(roomCM.MainPicture, "room");
            var subImageList = await _firebaseService.UploadMultipleFileAsync(roomCM.SubPicture, "room");

            addRoom.MainPicture = mainImageUrl;

            await _roomService.AddAsync(addRoom);
            var isAddedRoom = await _roomService.SaveChangeAsync();
            if (isAddedRoom)
            {
                foreach (var subImageUrl in subImageList)
                {
                    RoomImage roomImage = new RoomImage();
                    roomImage.Id = Guid.NewGuid().ToString();
                    roomImage.RoomId = roomId;
                    roomImage.ImageUrl = subImageUrl;
                    await _roomImageService.AddAsync(roomImage);
                }
                await _roomImageService.SaveChangeAsync();
                return Ok(new Response
                {
                    status = "Success",
                    message = "Create room successfully!",
                });
            }
            return BadRequest(new Response
            {
                status = "Failed",
                message = "Cannot create room!",
            });
        }


    }
}

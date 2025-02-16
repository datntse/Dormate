using AutoMapper;
using CMMS.API.Helpers;
using Dormate.API.Services;
using Dormate.Core.Entities;
using Dormate.Core.Models;
using Dormate.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.IdentityModel.Tokens;

namespace Dormate.API.Controllers
{
    [Route("api/room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IRoomService _roomService;
        private ICurrentUserService _currentUserService;
        private IRoomImageService _roomImageService;
        private IFirebaseService _firebaseService;
        private IMapper _mapper;

        public RoomController(IRoomService roomService, ICurrentUserService currentUserService,
            IRoomImageService roomImageService, IFirebaseService firebaseService, IMapper mapper)
        {
            _roomService = roomService;
            _currentUserService = currentUserService;
            _roomImageService = roomImageService;
            _firebaseService = firebaseService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] RoomFilterModel filterModel)
        {

            var filteredList = _roomService.Get(_ =>
                 (string.IsNullOrEmpty(filterModel.Id) || _.Equals(filterModel.Id)) &&
                 (string.IsNullOrEmpty(filterModel.Name) || _.Name.Contains(filterModel.Name)) &&
                 (string.IsNullOrEmpty(filterModel.Province) || _.Province.Equals(filterModel.Province)) &&
                 (string.IsNullOrEmpty(filterModel.Ward) || _.Ward.Equals(filterModel.Ward)) &&
                 (string.IsNullOrEmpty(filterModel.District) || _.District.Equals(filterModel.District)) &&
                 (string.IsNullOrEmpty(filterModel.Address) || _.District.Contains(filterModel.Address)) &&
                 (filterModel.Status != null || _.Status.Equals(filterModel.Status)));
            return Ok(filteredList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewRoomAsync([FromForm]RoomCM roomCM)
        {
            var userId = _currentUserService.GetUserId();

            if (roomCM.MainPicture == null || roomCM.MainPicture.Length == 0) {
                return BadRequest("File not provided!");
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
                return Ok(new
                {

                });
            }
            return BadRequest("Error in create room");
        }

   
    }
}

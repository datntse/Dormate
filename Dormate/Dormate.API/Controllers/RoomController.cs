using Dormate.API.Services;
using Dormate.Core.Models;
using Dormate.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public RoomController(IRoomService roomService, ICurrentUserService currentUserService,
            IRoomImageService roomImageService, IFirebaseService firebaseService)
        {
            _roomService = roomService;
            _currentUserService = currentUserService;
            _roomImageService = roomImageService;
            _firebaseService = firebaseService;
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
        public async Task<IActionResult> CreateNewRoomAsync(RoomCM roomCM)
        {
            
        }
    }
}

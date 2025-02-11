

using Dormate.Core.Constrants;
using Dormate.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Dormate.API.Services
{
    public interface ICurrentUserService
    {
        string GetUserId();
        Task<ApplicationUser> GetCurrentUser();
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string GetUserId() =>
             _httpContextAccessor.HttpContext.
				User.Claims.FirstOrDefault(_ => _.Type == CustomClaims.UserId)?.Value;
        public async Task<ApplicationUser> GetCurrentUser()
        {
            var userId = GetUserId();
            return await _userManager.FindByIdAsync(userId.ToString());
        }
  
    }
}

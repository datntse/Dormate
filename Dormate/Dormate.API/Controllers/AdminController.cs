
using AutoMapper;
using CMMS.API.Helpers;
using Dormate.Core.Constant;
using Dormate.Core.Models;
using Dormate.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SQLitePCL;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dormate.API.Controllers
{
    [Route("api/admin")]
    [AllowAnonymous]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IUserService _userService;
        private IRoleService _roleService;
        private IMapper _mapper;

        public AdminController(IRoleService roleService,
            IUserService userSerivce, IMapper mapper)
        {
            _userService = userSerivce;
            _roleService = roleService;
            _mapper = mapper;
        }
        #region userManagement
        [HttpGet("get-all-user")]
        public async Task<IActionResult> GetAllUser([FromQuery] DefaultSearch defaultSearch)
        {
            var result = await _userService.GetAll();
            var data = result.Sort(string.IsNullOrEmpty(defaultSearch.sortBy) ? "UserName" : defaultSearch.sortBy
                      , defaultSearch.isAscending)
                      .ToPageList(defaultSearch.currentPage, defaultSearch.perPage).AsNoTracking().ToList();
            return Ok(new { total = result.ToList().Count, data, page = defaultSearch.currentPage, perPage = defaultSearch.perPage });
        }

        [HttpPost("add-new-user")]
        public async Task<IActionResult> AddNewUser(UserCM userCM)
        {
            var result = await _userService.AddAsync(userCM);
            return Ok(result);
        }
        #endregion


        #region roles

        [HttpGet("get-roles")]
        public async Task<IActionResult> GetAccounts()
        {
            var result = await _roleService.GetRole();
            var listRoles = result.Select(_ => new
            {
                _.Id,
                _.Name
            });
            return Ok(listRoles);
        }

        [HttpGet("get-role/{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var result = await _roleService.GetRoleById(id);
            if (result != null) { return Ok(result); }
            return BadRequest("Cannot found");
        }

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var result = await _roleService.CreateRole(roleName);
            return Ok(result);
        }

        [HttpPut("update-role/{id}")]
        public async Task<IActionResult> UpdateRole(string roleName, string id)
        {
            var result = await _roleService.UpdateRole(roleName, id);
            if (result > 0) return Ok();
            return BadRequest("Cannot update");
        }

        [HttpDelete("delete-role/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var result = await _roleService.DeleteRole(roleId);
            return Ok(result);
        }

        [HttpGet("get-user-role/{userId}")]
        public async Task<IActionResult> GetUserRole(string userId)
        {
            var result = await _roleService.GetUserRole(userId);
            if (result != null) return Ok(result);
            return BadRequest("Cannot found");
        }


        [HttpPost("add-user-to-role")]
        public async Task<IActionResult> AddRoleUser(List<string> roleNames, string userId)
        {
            var result = await _roleService.AddRoleUser(roleNames, userId);
            return Ok(result);
        }


        #endregion

        

        #region seeding
        [HttpGet("SeedRole")]
        public async Task<IActionResult> SeedRoleAsync()
        {
            try
            {
                await _roleService.SeedingRole();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok();
        }
        #endregion


    }
}

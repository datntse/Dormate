using AutoMapper;
using Dormate.Core.Entities;
using Dormate.Core.Enums;
using Dormate.Core.Models;
using Dormate.Infrastructure.Data;
using Dormate.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;


namespace Dormate.Infrastructure.Services
{
    public interface IRoleService
    {
        Task<List<IdentityRole>> GetRole();
        Task<IdentityRole> GetRoleById(String id);
        Task<IdentityResult> CreateRole(String roleName);
        Task<int> UpdateRole(String roleName, String id);
        Task<IdentityResult> DeleteRole(String roleId);
        Task<String[]> GetUserRole(string userId);
        Task<IdentityResult> AddRoleUser(List<string> roleNames, String userId);
        Task<List<UserRolesVM>> GetListUsers();
        Task SeedingRole();
    }
    public class RoleService : IRoleService
    {
        private ILogger _logger;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransaction _efTransaction;

        public RoleService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration,
            RoleManager<IdentityRole> roleManager, IMapper mapper, ApplicationDbContext dbContext,
            IUnitOfWork unitOfWork, IRoleRepository roleRepository,
            IUserRepository userRepository, ILogger<RoleService> logger, ITransaction transaction)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;

            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _efTransaction = transaction;
        }

        public async Task<List<IdentityRole>> GetRole()
        {
            return await _roleManager.Roles.OrderBy(_ => _.Name).ToListAsync();
        }

        public async Task<IdentityRole> GetRoleById(string id)
        {
            return await _roleManager.FindByIdAsync(id);

        }

        public async Task<IdentityResult> CreateRole(string roleName)
        {
            IdentityRole _roleName = new IdentityRole(roleName);
            return await _roleManager.CreateAsync(_roleName);
        }

        public async Task<int> UpdateRole(string roleName, string id)
        {
            var role = await _roleRepository.FindAsync(id);
            if (role != null)
            {
                role.Name = roleName;
                _roleRepository.Update(role);
                if (await _unitOfWork.SaveChangeAsync()) return 1;
            }
            return 0;
        }

        public async Task<IdentityResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            return await _roleManager.DeleteAsync(role);
        }

        public async Task<String[]> GetUserRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return (await _userManager.GetRolesAsync(user)).ToArray<string>();
        }

        public async Task<IdentityResult> AddRoleUser(List<string> roleNames, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = (await _userManager.GetRolesAsync(user)).ToArray<string>();
            var deleteRoles = userRoles.Where(r => !roleNames.Contains(r));
            var addRoles = roleNames.Where(r => !userRoles.Contains(r));
            var result = await _userManager.RemoveFromRolesAsync(user, deleteRoles);
            return result = await _userManager.AddToRolesAsync(user, addRoles);
        }

        public async Task<List<UserRolesVM>> GetListUsers()
        {
            var userList = _userRepository.GetAll();
            var userTotal = userList.Select(_ => new UserRoles { Id = _.Id }).ToListAsync();
            var users = await userList.Select(_ => new UserRoles
            {
                Id = _.Id,
                UserName = _.UserName,
                Email = _.Email,
                FullName = _.FullName,
            }).ToListAsync();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RolesName = roles.ToList<string>();
            }
            var result = users.Select(_ => _mapper.Map<UserRoles, UserRolesVM>(_));
            return result.ToList();
        }

        public async Task SeedingRole()
        {
            foreach (Roles role in Enum.GetValues(typeof(Roles)))
            {
                if (_roleRepository.Get(r => r.Name.Equals(role.ToString())).FirstOrDefault() == null)
                {
                    await _roleRepository.AddAsync(new IdentityRole(role.ToString())
                    {
                        NormalizedName = role.ToString().ToUpper()
                    });
                }
            }
            var result =  await _unitOfWork.SaveChangeAsync();
            if (result)
                await _efTransaction.CommitAsync();
        }


        public List<string> getRolePermission<T>(T rolePermission)
        {
            List<string> rolePermissons = new List<string>();
            foreach (T role in Enum.GetValues(typeof(T)))
            {
                rolePermissons.Add(role.ToString());
            }
            return rolePermissons;
        }
    
    }
}

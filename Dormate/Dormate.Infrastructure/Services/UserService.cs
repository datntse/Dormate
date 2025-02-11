using AutoMapper;
using Dormate.Core.Constant;
using Dormate.Core.Entities;
using Dormate.Core.Enums;
using Dormate.Core.Models;
using Dormate.Infrastructure.Data;
using Dormate.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq.Expressions;


namespace Dormate.Infrastructure.Services
{
    public interface IUserService
    {
        #region CRUD
        Task<Message> CustomerSignUpAsync(UserDTO model);
        Task<ApplicationUser> SignInAsync(UserSignIn model);
        Task<Message> AddAsync(UserCM user);
        Task<IList<String>> GetRolesAsync(ApplicationUser user);
        Task<ApplicationUser> FindAsync(Guid id);
        Task<ApplicationUser> FindbyEmail(String email);
        Task<ApplicationUser> FindByUserName(String userName);
        IQueryable<ApplicationUser> Get(Expression<Func<ApplicationUser, bool>> where);
        IQueryable<ApplicationUser> Get(Expression<Func<ApplicationUser, bool>> where, params Expression<Func<ApplicationUser, object>>[] includes);
        IQueryable<ApplicationUser> Get(Expression<Func<ApplicationUser, bool>> where, Func<IQueryable<ApplicationUser>, IIncludableQueryable<ApplicationUser, object>> include = null);
        void Update(ApplicationUser user);
        Task<bool> CheckExist(Expression<Func<ApplicationUser, bool>> where);
        Task<bool> SaveChangeAsync();
        Task<IQueryable<UserRolesVM>> GetAll();
        #endregion
        Task<bool> ConfirmAccount(string email);
        Task<ApplicationUser> FindAsync(string customerId);
        Task<IdentityResult> UpdateAnsyc(ApplicationUser updateUser);
        Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
        string GenerateCustomerCode();

    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration,
            RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        #region CRUD
        public async Task<ApplicationUser> SignInAsync(UserSignIn model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                return user;
            }
            return null;
        }

        public async Task<IQueryable<UserRolesVM>> GetAll()
        {
            var listUserRolesVM = new List<UserRolesVM>();
            var listUser = _userRepository.GetAll().ToList();
            foreach (var user in listUser.ToList())
            {
                var userRoles = (await GetRolesAsync(user));
                var userRolesVM = _mapper.Map<UserRolesVM>(user);
                userRolesVM.RolesName = userRoles.ToList();
                listUserRolesVM.Add(userRolesVM);
                if (userRoles.Contains(Roles.Admin.ToString()))
                {
                    listUser.Remove(user);
                }
            }
            return listUserRolesVM.AsQueryable();
        }


        public async Task<Message> CustomerSignUpAsync(UserDTO model)
        {
            var result = new Message();

            var isDupplicate = await _userManager.FindByEmailAsync(model.Email);
            if (isDupplicate != null)
            {
                result.StatusCode = 400;
                result.Content = "Email đã được sử dụng";
                return result;
            }
            var user = _mapper.Map<ApplicationUser>(model);
            user.Id = GenerateCustomerCode();
            IdentityResult identityResult = null;
            if (model.Password != null)
                identityResult = await _userManager.CreateAsync(user, model.Password);
            else
                identityResult = await _userManager.CreateAsync(user);

            if (identityResult.Succeeded)
            {
                if (model.Type.Equals((int)Roles.Tenant))
                {
                    await _userManager.AddToRoleAsync(user, Roles.Tenant.ToString());
                } else if (model.Type.Equals((int)Roles.Landlord))
                {
                    await _userManager.AddToRoleAsync(user, Roles.Landlord.ToString());
                }
                result.StatusCode = 200;
                result.Content = "Tạo tài khoản thành công, chúng tôi cần bạn xác thực tài khoản qua email đã đăng kí";
                return result;
            }

            return result;
        }

        public async Task<Message> AddAsync(UserCM userCM)
        {
            var message = new Message();
            var isDupplicate = await _userManager.FindByEmailAsync(userCM.Email);
            if (isDupplicate != null)
            {
                message.Content = "Email is already in use";
                return message;
            }
            var user = _mapper.Map<ApplicationUser>(userCM);
            user.Id = GenerateCustomerCode();
            IdentityResult result = IdentityResult.Success;
            user.Id = GenerateCustomerCode();
            if (userCM.Password != null)
            {
                result = await _userManager.CreateAsync(user, userCM.Password);
            }
            else
            {
                // login by google EmailConfirmed is true
                user.EmailConfirmed = true;
                result = await _userManager.CreateAsync(user);
                var loginProviderInfo = new UserLoginInfo(userCM.LoginProvider, userCM.ProviderKey, userCM.ProviderDisplayName);
                result = await _userManager.AddLoginAsync(user, loginProviderInfo);
            }

            if (result.Succeeded)
            {
                var roleName = userCM.RoleName;
                var isExistedRole = await _roleManager.FindByNameAsync(roleName);
                if (isExistedRole == null)
                {
                    message.Content = "Role not found";
                    return message;
                }
                else await _userManager.AddToRoleAsync(user, roleName);
                message.Content = "Add new user successfully";
                message.StatusCode = 201;
            }
            await _unitOfWork.SaveChangeAsync();
            return message;
        }

        public async Task<bool> CheckExist(Expression<Func<ApplicationUser, bool>> where)
        {
            return await _userRepository.CheckExist(where);
        }

        public async Task<ApplicationUser> FindAsync(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public IQueryable<ApplicationUser> Get(Expression<Func<ApplicationUser, bool>> where)
        {
            return _userRepository.Get(where);
        }

        public IQueryable<ApplicationUser> Get(Expression<Func<ApplicationUser, bool>> where, params Expression<Func<ApplicationUser, object>>[] includes)
        {
            return _userRepository.Get(where, includes);
        }

        public IQueryable<ApplicationUser> Get(Expression<Func<ApplicationUser, bool>> where, Func<IQueryable<ApplicationUser>, IIncludableQueryable<ApplicationUser, object>> include = null)
        {
            return _userRepository.Get(where, include);
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _unitOfWork.SaveChangeAsync();
        }

        public void Update(ApplicationUser user)
        {
            _userRepository.Update(user);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }



        public async Task<ApplicationUser> FindbyEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> FindByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<bool> ConfirmAccount(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;
            user.EmailConfirmed = true;
            _userRepository.Update(user);
            return await _unitOfWork.SaveChangeAsync();
        }

        public async Task<ApplicationUser> FindAsync(string customerId)
        {
            return await _userRepository.FindAsync(customerId);
        }

        public async Task<bool> IsEmailConfirmedAsync(ApplicationUser user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public string GenerateCustomerCode()
        {
            var userTotal = _userRepository.GetAll();
            string userId = $"KH{(userTotal.Count() + 1):D6}";
            return userId;
        }

        #endregion

        #region customer tracking revenue

        public async Task<IdentityResult> UpdateAnsyc(ApplicationUser updateUser)
        {
            return await _userManager.UpdateAsync(updateUser);
        }

        #endregion
    }
}

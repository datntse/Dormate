﻿using CMMS.API.Helpers;
using Dormate.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using AutoMapper;
using Dormate.API.Services;
using Dormate.Infrastructure.Data;
using Dormate.Infrastructure.Services;
using Dormate.Core.Constant;

namespace Dormate.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        //private readonly HttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthenticationController(IJwtTokenService jwtTokenService,
            IUserService userService,
            ICurrentUserService currentUserService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IMailService mailService, IConfiguration configuration)
        {
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
            _userService = userService;
            _currentUserService = currentUserService;
            //_httpClient = httpClient;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mailService = mailService;

        }

        [AllowAnonymous]
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp(UserDTO signUpModel)
        {
            var emailExist = await _userService.FindbyEmail(signUpModel.Email);
            var userNameExist = await _userService.FindByUserName(signUpModel.UserName);
            if (emailExist != null)
            {
                return BadRequest("Email đã được sử dụng");
            }
            else if (userNameExist != null)
            {
                return BadRequest("User đã được sử dụng");
            }

            var result = await _userService.CustomerSignUpAsync(signUpModel);
            var url = Url.Action(nameof(ConfirmAccount), nameof(AuthenticationController).Replace("Controller", ""), null, Request.Scheme);
            if (result.StatusCode == 200)
                url += $"?email={signUpModel.Email}";
            await _mailService.SendEmailAsync(signUpModel.Email, "Xác thực tài khoản của bạn", url);
            return Ok(new
            {
                status = "success",
                message =  result.Content
            });
        }

        [AllowAnonymous]
        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn(UserSignIn signIn)
        {
            var user = await _userService.SignInAsync(signIn);
            if (user == null)
            {
                return NotFound("Sai tên đăng nhập hoặc mật khẩu");
            }
            else if (user.Status == 0)
            {
                return BadRequest("Tài khoản của bạn bị vô hiệu hóa");
            }
            else if (user.EmailConfirmed == false)
            {
                return BadRequest("Tài khoản của bạn chưa được xác nhận, vui lòng confirm qua email của bạn!");
            }

            var userRoles = await _userService.GetRolesAsync(user);
            var accessToken = await _jwtTokenService.CreateToken(user, userRoles);
            var refreshToken = _jwtTokenService.CreateRefeshToken();
            user.RefreshToken = refreshToken;
            user.DateExpireRefreshToken = TimeConverter.GetVietNamTime().AddDays(7);
            _userService.Update(user);
            var result = await _userService.SaveChangeAsync();
            if (result)
            {
                return Ok(new
                {
                    data = new
                    {
                        accessToken,
                        refreshToken,
                    }
                });
            }
            return BadRequest("Failed to update user's token");
        }

        [HttpDelete("signOut")]
        public async Task<IActionResult> SignOut()
        {
            var user = await _currentUserService.GetCurrentUser();
            if (user is null)
                return Unauthorized();
            user.RefreshToken = null;
            _userService.Update(user);
            await _userService.SaveChangeAsync();
            return Ok();
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> refeshToken(string refreshToken)
        {
            var userId = _currentUserService.GetUserId();
            var user = await _userService.FindAsync(Guid.Parse(userId));
            if (user == null || !(user.Status != 0) || user.RefreshToken != refreshToken || user.DateExpireRefreshToken < DateTime.UtcNow)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return BadRequest(new Message
                {
                    Content = "Not permission",
                    StatusCode = 404
                });
            }
            var userRoles = await _userService.GetRolesAsync(user);
            var newRefreshToken = _jwtTokenService.CreateRefeshToken();
            user.RefreshToken = newRefreshToken;
            user.DateExpireRefreshToken = TimeConverter.GetVietNamTime().AddDays(7);
            var accessToken = await _jwtTokenService.CreateToken(user, userRoles);
            _userService.Update(user);
            await _userService.SaveChangeAsync();
            return Ok(new
            {
                data = new
                {
                    accessToken,
                    refreshToken = newRefreshToken
                }
                }
            );
        }

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmAccount([FromQuery] string email)
        {
            var result = await _userService.ConfirmAccount(email);
            if (result)
                return Redirect("https://cmms-customer-app.vercel.app/login");
            return BadRequest("Có lỗi trong lúc confirm email");
        }

    }
}

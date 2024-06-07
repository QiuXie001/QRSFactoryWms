using DB.Models;
using Humanizer;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Security;
using Services;
using SqlSugar;
using System.Linq.Expressions;
using System.Security.Claims;
using Qiu.Utils.Jwt;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace QRSFactoryWmsAPI.Controllers.Sys
{
    public class AuthenticationController : BaseController
    {
        private readonly ISys_UserService _userService;
        private readonly ISys_LogService _logService;
        private readonly ISys_IdentityService _identityService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;
        private readonly Jwt _jwt;

        public AuthenticationController(
            Xss xss,
            ISys_LogService logService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            ISys_UserService userService,
            ISys_IdentityService identityService,
            IMediator mediator,
            Jwt jwt)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _userService = userService;
            _identityService = identityService;
            _logService = logService;
            _xss = xss;
            _mediator = mediator;
            _jwt = jwt;

        }


        [HttpGet]
        [EnableCors]
        [AllowAnonymous]
        [Route("Authentication/CheckLogin")]
        public async Task<IActionResult> CheckLogin()
        {
            string clientIp = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            // 处理代理服务器
            if (clientIp.Equals("127.0.0.1")) // 假设代理服务器IP地址是127.0.0.1
            {
                clientIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            var user = new SysUser
            {
                UserName = "admin",
                Pwd = "12345678",
                LoginIp = clientIp
            };
            var flag = await _userService.CheckLoginAsync(user);
            if (flag.Item1)
            {
                string token = await _identityService.GenerateToken(flag.Item3.UserId);
                return new JsonResult((flag, token));
            }
            return new JsonResult(flag);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [AllowAnonymous]
        [Route("Authentication/CheckLogin")]
        public async Task<IActionResult> CheckLogin(string username, string password)
        {
            var user = new SysUser
            {
                UserName = username,
                Pwd = password,
                LoginDate = DateTime.Now.ToString("D").ToDateTime(),
                LoginTime = DateTime.Now.ToString("HHmmss").ToInt32()
            };
            var flag = await _userService.CheckLoginAsync(user);
            if (flag.Item1)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                var claims = new List<Claim>
                  {
                      new Claim(ClaimTypes.Name, flag.Item3.UserName),
                      new Claim(ClaimTypes.Sid, flag.Item3.UserId.ToString()),
                      new Claim(ClaimTypes.Surname, flag.Item3.UserNickname),
                      new Claim(ClaimTypes.Role, flag.Item3.RoleId.ToString()),
                      new Claim(ClaimTypes.Uri, string.IsNullOrWhiteSpace(flag.Item3.HeadImg)?Path.Combine("upload","head","4523c812eb2047c39ad91f8c5de3fb31.jpg"):flag.Item3.HeadImg)
                  };
                var claimsIdentitys = new ClaimsIdentity(
               claims,
               CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentitys);
                Task.Run(async () =>
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
                    {
                        IssuedUtc = DateTime.Now,
                        IsPersistent = true,
                        AllowRefresh = true,
                        ExpiresUtc = DateTime.Now.AddDays(1),
                    });
                }).Wait();
                user.UserId = flag.Item3.UserId;
                user.UserName = flag.Item3.UserName;
                user.UserNickname = flag.Item3.UserNickname;
                user.RoleId = flag.Item3.RoleId;
                user.HeadImg = flag.Item3.HeadImg;
                GetMemoryCache.Set("user_" + flag.Item3.UserId, user);

                await _logService.InsertAsync(new SysLog
                {
                    LogId = PubId.SnowflakeId,
                    Browser = GetBrowser(),
                    CreateBy = flag.Item3.UserId,
                    CreateDate = DateTime.Now,
                    Description = $"{flag.Item3.UserNickname}登录成功",
                    LogIp = GetIp(),
                    Url = GetUrl(),
                    LogType = LogType.login.EnumToString(),
                });
                string token = await _identityService.GenerateToken(flag.Item3.UserId);
                return new JsonResult((flag, token));
            }
            else
            {
                await _logService.InsertAsync(new SysLog
                {
                    LogId = PubId.SnowflakeId,
                    Browser = GetBrowser(),
                    CreateDate = DateTime.Now,
                    Description = $"{_xss.Filter(user.UserNickname)}登录失败",
                    LogIp = GetIp(),
                    Url = GetUrl(),
                    LogType = LogType.login.EnumToString()
                });
                return new JsonResult(flag);
            }
        }


    }
}

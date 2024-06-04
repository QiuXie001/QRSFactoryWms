using DB.Models;
using Humanizer;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Security;
using Services;
using SqlSugar;
using System.Linq.Expressions;

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

        public AuthenticationController(
            Xss xss,
            ISys_LogService logService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            ISys_UserService userService,
            ISys_IdentityService identityService,
            IMediator mediator)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _userService = userService;
            _identityService = identityService;
            _logService = logService;
            _xss = xss;
            _mediator = mediator;

        }


        [HttpGet]
        [Authorize]
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
                Pwd = "123456",
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
        [Authorize]
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

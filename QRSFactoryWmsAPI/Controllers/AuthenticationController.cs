using DB.Models;
using Humanizer;
using IServices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Json;
using Qiu.Utils.Security;
using System.Linq.Expressions;

namespace QRSFactoryWmsAPI.Controllers
{
    public class AuthenticationController : BaseController
    {
        private readonly ISys_UserService _userServices;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ISys_LogService _logService;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AuthenticationController(
            Xss xss,
            ISys_LogService logService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            ISys_UserService userServices,
            IMediator mediator,
            SignInManager<IdentityUser> signInManager)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _userServices = userServices;
            _logService = logService;
            _xss = xss;
            _mediator = mediator;
            _signInManager = signInManager;

        }
        [HttpGet]
        [AllowAnonymous]
        [Route("Authentication/CheckLogin")]
        public async Task<string> CheckLogin()
        {
            var user = new SysUser
            {
                UserName = "admin",
                Pwd = "kopsoft",
            };
            var flag = await _userServices.CheckLoginAsync(user);
             return flag.ToJson();
        }
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("Authentication/CheckLogin")]
        //public async Task<string> CheckLogin(string username ,string password)
        //{
        //    var user = new SysUser
        //    {
        //        UserId = userid,
        //        Pwd = password
        //    }; 
        //    var flag = await _userServices.CheckLogin(user);

        //    return flag.ToJson();
        //}
    }
}

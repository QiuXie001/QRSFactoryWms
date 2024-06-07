﻿using DB.Models;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NetTaste;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Security;
using Qiu.Utils.Table;

namespace QRSFactoryWmsAPI.Controllers
{
    public class UserController : BaseController
    {
        private readonly ISys_UserService _userService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ISys_LogService _logService;
        private readonly ISys_IdentityService _identityService;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;
        private readonly string NowUrl = "/User";

        public UserController(Xss xss, 
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
            _logService = logService;
            _identityService = identityService;
            _xss = xss;
            _mediator = mediator;

        }
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("User/GetUsers")]
        public async Task<string> GetUsers()
        {
            var bootstrap = PubConst.DefaultBootstrapParams;

            var item = await _userService.PageListAsync(bootstrap);

            return item;
        }
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("User/GetPageList")]
        public async Task<string> GetPageList(Bootstrap.BootstrapParams bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return (false, PubConst.ValidateToken2).ToJson();
            }
            if (bootstrap._ == null)
                bootstrap =PubConst.DefaultBootstrapParams;
            var item = await _userService.PageListAsync(bootstrap);
            return item;
        }
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.add)]
        [Route("User/Insert")]
        public async Task<IActionResult> Insert(SysUser user, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var item = await _userService.InsertAsync(user);
            return new JsonResult((item, PubConst.Add1));
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("User/Update")]
        public async Task<IActionResult> Update(SysUser user, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var item = await _userService.InsertAsync(user);
            return new JsonResult((item, PubConst.Add1));
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("User/Delete")]
        public async Task<IActionResult> Delete(SysUser user, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var item = await _userService.InsertAsync(user);
            return new JsonResult((item, PubConst.Add1));
        }
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.disable)]
        [Route("User/Disable")]
        public async Task<IActionResult> Disable(SysUser user, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var item = await _userService.InsertAsync(user);
            return new JsonResult((item, PubConst.Add1));
        }
    }
}

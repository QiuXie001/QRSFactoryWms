﻿using DB.Dto;
using DB.Models;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Security;
using Qiu.Utils.Table;
using SqlSugar;

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
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("User/GetPageList")]
        public async Task<string> GetPageList([FromForm] string bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return (false, PubConst.ValidateToken2).ToJson();
            }

            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var item = await _userService.PageListAsync(bootstrapObject);
            return item;
        }
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.add)]
        [Route("User/Add")]
        public async Task<IActionResult> Add([FromForm] string user, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var userObject = JsonConvert.DeserializeObject<SysUser>(user);
            userObject.UserId = PubId.SnowflakeId;
            userObject.HeadImg = "null";
            userObject.Pwd = userObject.Pwd.ToMd5();
            userObject.Sex = userObject.Sex;
            userObject.CreateBy = userId;
            userObject.CreateDate = DateTime.UtcNow;
            userObject.ModifiedBy = userId;
            userObject.ModifiedDate = DateTime.UtcNow;
            userObject.LoginDate = DateTime.UtcNow;
            userObject.IsDel = 1;
            userObject.IsEabled = 1;
            var item = await _userService.InsertAsync(userObject);
            return Ok((item, PubConst.Add1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("User/Update")]
        public async Task<IActionResult> Update([FromForm] string user, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var userObject = JsonConvert.DeserializeObject<UserDto>(user);
            var entity = await _userService.QueryableToSingleAsync(u => u.UserId == userObject.UserId && u.IsDel == 1);
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.Now;
            entity.UserName = userObject.UserName;
            entity.UserNickname = userObject.UserNickname;
            entity.Pwd = userObject.Pwd.ToMd5();
            entity.Sex = userObject.Sex;
            entity.DeptId = userObject.DeptId;
            entity.RoleId = userObject.RoleId;
            entity.Remark = userObject.Remark;
            var item = await _userService.UpdateAsync(entity);
            return Ok((item, PubConst.Update1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("User/Delete")]
        public async Task<IActionResult> Delete([FromForm] string user, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var item = await _userService.DeleteAsync(user);
            return Ok((item, PubConst.Delete1));
        }
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.disable)]
        [Route("User/Disable")]
        public async Task<IActionResult> Disable([FromForm] string user, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var userObject = JsonConvert.DeserializeObject<SysUser>(user);
            var item = await _userService.Disable(userObject, userId);
            return Ok((item, PubConst.Delete1));
        }

        

        
    }
}

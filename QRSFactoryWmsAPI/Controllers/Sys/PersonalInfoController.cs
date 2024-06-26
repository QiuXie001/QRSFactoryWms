using DB.Dto;
using DB.Models;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qiu.NetCore.Attributes;
using Qiu.Utils.Env;
using Qiu.Utils.Pub;
using Qiu.Utils.Security;

namespace QRSFactoryWmsAPI.Controllers.Sys
{
    public class PersonalInfoController : Controller
    {
        private readonly ISys_UserService _userService;
        private readonly ISys_RoleService _roleService;
        private readonly ISys_DeptService _deptService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ISys_LogService _logService;
        private readonly ISys_IdentityService _identityService;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;
        private readonly string NowUrl = "#";
        public PersonalInfoController(Xss xss,
           ISys_LogService logService,
           IHttpContextAccessor httpContext,
           IConfiguration configuration,
           ISys_UserService userService,
           ISys_RoleService roleService,
           ISys_DeptService deptService,
           ISys_IdentityService identityService,
           IMediator mediator)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _userService = userService;
            _roleService = roleService;
            _deptService = deptService;
            _logService = logService;
            _identityService = identityService;
            _xss = xss;
            _mediator = mediator;

        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [Route("PersonalInfo/GetInfo")]
        public async Task<IActionResult> GetInfoAsync(string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return NotFound((false, PubConst.ValidateToken2));
            }
            var item = await _userService.QueryableToSingleAsync(u => u.UserId == userId && u.IsDel == 1);
            UserDto user = new UserDto()
            {
                HeadImg = item.HeadImg,
                UserId = item.UserId,
                UserName = item.UserName,
                UserNickname = item.UserNickname,
                Pwd = item.Pwd,
                Email = item.Email,
                Tel = item.Tel,
                Mobile = item.Mobile,
                Sex = item.Sex,
                RoleId = item.RoleId,
                RoleName = await _roleService.GetRoleNameById(item.RoleId),
                DeptId = item.DeptId,
                DeptName = await _deptService.GetDeptNameById(item.DeptId),
                LoginDate = item.LoginDate,
                Remark = item.Remark,
                Sort = item.Sort,
            };

            return Ok((true, user));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("PersonalInfo/Update")]
        public async Task<IActionResult> UpdateAsync([FromForm] string user, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var userObject = JsonConvert.DeserializeObject<UserDto>(user);

            SysUser entity = await _userService.QueryableToSingleAsync(u => u.UserId == userId && u.IsDel == 1);

            entity.HeadImg = userObject.HeadImg;
            entity.UserName = userObject.UserName;
            entity.UserNickname = userObject.UserNickname;
            entity.Pwd = userObject.Pwd.ToMd5();
            entity.Sex = userObject.Sex;
            entity.DeptId = userObject.DeptId;
            entity.Tel = userObject.Tel;
            entity.Email = userObject.Email;
            entity.Remark = userObject.Remark;


            var item = await _userService.UpdateAsync(entity);
            return Ok((item, PubConst.Update1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("PersonalInfo/Delete")]
        public async Task<IActionResult> DeleteAsync([FromForm] string user, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var userObject = JsonConvert.DeserializeObject<SysUser>(user);
            userObject.IsDel = 0;
            userObject.ModifiedBy = userId;
            userObject.ModifiedDate = DateTime.Now;
            var item = await _userService.UpdateAsync(userObject);
            return Ok((item, PubConst.Add1));
        }
    }
}

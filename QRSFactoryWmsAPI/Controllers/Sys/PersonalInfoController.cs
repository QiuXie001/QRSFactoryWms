using DB.Models;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Qiu.NetCore.Attributes;
using Qiu.Utils.Pub;
using Qiu.Utils.Security;

namespace QRSFactoryWmsAPI.Controllers.Sys
{
    public class PersonalInfoController : Controller
    {
        private readonly ISys_UserService _userService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ISys_LogService _logService;
        private readonly ISys_IdentityService _identityService;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;
        private readonly string NowUrl = "/#";
        public PersonalInfoController(Xss xss,
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
        [Route("PersonalInfo/GetInfo")]
        public async Task<IActionResult> GetInfoAsync(string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var item = await _userService.QueryableToSingleAsync(u => u.UserId == userId && u.IsDel ==1);
            return new JsonResult(item);
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("PersonalInfo/Update")]
        public async Task<IActionResult> UpdateAsync(SysUser user, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            user.UserId = userId;
            user.ModifiedBy = userId;
            user.ModifiedDate = DateTime.Now;
            var item = await _userService.UpdateAsync(user);
            return new JsonResult((item, PubConst.Update1));
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("PersonalInfo/Delete")]
        public async Task<IActionResult> DeleteAsync(SysUser user, string token, long userId)
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

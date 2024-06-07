using DB.Models;
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

namespace QRSFactoryWmsAPI.Controllers.Sys
{
    public class PermissionController : BaseController
    {
        private readonly ISys_RoleService _roleServices;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ISys_LogService _logService;
        private readonly ISys_UserService _userService;
        private readonly ISys_IdentityService _identityService;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;
        private readonly string NowUrl = "#";

        public PermissionController(
            Xss xss, 
            ISys_LogService logService, 
            IHttpContextAccessor httpContext, 
            IConfiguration configuration, 
            ISys_RoleService roleServices,
            ISys_UserService userService,
            ISys_IdentityService identityService,
            IMediator mediator)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _roleServices = roleServices;
            _logService = logService;
            _userService = userService;
            _identityService = identityService;
            _xss = xss;
            _mediator = mediator;
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("Permission/GetPermissions")]
        public async Task<IActionResult> GetPermissions(long userId, string token )
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var roleId = await _userService.GetRoleAsync(userId);

            var roleMenu = await _roleServices.GetMenuAsync(roleId);
            return new JsonResult(roleMenu);
        }
    }
}

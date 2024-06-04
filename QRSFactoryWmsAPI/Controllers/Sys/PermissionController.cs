using DB.Models;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTaste;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Json;
using Qiu.Utils.Security;

namespace QRSFactoryWmsAPI.Controllers.Sys
{
    public class PermissionController : BaseController
    {
        private readonly ISys_RoleService _roleServices;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ISys_LogService _logService;
        private readonly ISys_UserService _userService;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;

        public PermissionController(
            Xss xss, 
            ISys_LogService logService, 
            IHttpContextAccessor httpContext, 
            IConfiguration configuration, 
            ISys_RoleService roleServices,
            ISys_UserService userService,
            IMediator mediator)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _roleServices = roleServices;
            _logService = logService;
            _userService = userService;
            _xss = xss;
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Permission/GetPermissions")]
        public async Task<IActionResult> GetPermissions(long userId/*,string token = null*/)
        {
            var roleId = await _userService.GetRoleAsync(userId);

            var roleMenu = await _roleServices.GetMenuAsync(roleId);

            return new JsonResult(roleMenu);
        }
    }
}

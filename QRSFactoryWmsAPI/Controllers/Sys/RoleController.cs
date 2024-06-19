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
using Qiu.Utils.Table;
using System.Data;

namespace QRSFactoryWmsAPI.Controllers.Sys
{
    public class RoleController : BaseController
    {
        private readonly ISys_RoleService _roleService;
        private readonly ISys_LogService _logService;
        private readonly ISys_IdentityService _identityService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;
        private readonly string NowUrl = "/Role";
        public RoleController(Xss xss,
            ISys_RoleService roleService,
            ISys_LogService logService,
            ISys_IdentityService identityService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            IMediator mediator)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _roleService = roleService;
            _logService = logService;
            _identityService = identityService;
            _xss = xss;
            _mediator = mediator;
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("Role/GetPageList")]
        public async Task<string> GetPageList(Bootstrap.BootstrapParams bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return (false, PubConst.ValidateToken2).ToJson();
            }
            if (bootstrap._ == null)
                bootstrap = PubConst.DefaultBootstrapParams;
            var item = await _roleService.PageListAsync(bootstrap);
            return item;
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.add)]
        [Route("Role/InsertRole")]
        public async Task<IActionResult> InsertRole(string token, long userId, SysRole role, string[] menuId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            bool flag = false;
            flag = await _roleService.InsertRole(role, userId, menuId);
            return new JsonResult((flag, PubConst.Add1));

        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("Role/UpdateRole")]
        public async Task<IActionResult> UpdateRole(string token, long userId, SysRole role, string[] menuId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            bool flag = false;
            flag = await _roleService.UpdateRole(role, userId, menuId);
            return new JsonResult((flag, PubConst.Update1));

        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Role/DeleteRole")]
        public async Task<IActionResult> DeleteRole(string token, long userId, SysRole role)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            bool flag = false;
            flag = await _roleService.DeleteRole(role);
            return new JsonResult((flag, PubConst.Delete1));

        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.disable)]
        [Route("Role/DisableRole")]
        public async Task<IActionResult> DisableRole(string token, long userId, SysRole role)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            bool flag = false;
            flag = await _roleService.DisableRole(role, userId);
            return new JsonResult((flag, PubConst.Enable3));

        }

        
    }
}

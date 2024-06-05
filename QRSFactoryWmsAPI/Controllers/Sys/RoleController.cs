using DB.Models;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTaste;
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

        [HttpGet]
        [AllowAnonymous]
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
            await _logService.InsertAsync(new SysLog
            {
                LogId = PubId.SnowflakeId,
                Browser = GetBrowser(),
                CreateBy = userId, 
                CreateDate = DateTime.UtcNow,
                Description = userId + "用户获取角色分页列表",
                LogIp = GetIp(),
                Url = GetUrl(),
                LogType = LogType.getList.ToString()
            });
            return item;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Role/InsertRole")]
        public async Task<IActionResult> InsertRole(string token, long userId, SysRole role, string[] menuId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            bool flag = false;
            flag = await _roleService.InsertRole(role, userId, menuId);
            await _logService.InsertAsync(new SysLog
            {
                LogId = PubId.SnowflakeId,
                Browser = GetBrowser(),
                CreateBy = userId,
                CreateDate = DateTime.UtcNow,
                Description = userId + "用户增加新角色" + role.RoleName,
                LogIp = GetIp(),
                Url = GetUrl(),
                LogType = LogType.add.ToString()
            });
            return new JsonResult((flag, PubConst.Add1));

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Role/UpdateRole")]
        public async Task<IActionResult> UpdateRole(string token, long userId, SysRole role, string[] menuId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            bool flag = false;
            flag = await _roleService.UpdateRole(role, userId, menuId);
            await _logService.InsertAsync(new SysLog
            {
                LogId = PubId.SnowflakeId,
                Browser = GetBrowser(),
                CreateBy = userId,
                CreateDate = DateTime.UtcNow,
                Description = userId + "用户更新角色" + role.RoleName,
                LogIp = GetIp(),
                Url = GetUrl(),
                LogType = LogType.update.ToString()
            });
            return new JsonResult((flag, PubConst.Update1));

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Role/DeleteRole")]
        public async Task<IActionResult> DeleteRole(string token, long userId, SysRole role)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            bool flag = false;
            flag = await _roleService.DeleteRole(role);
            await _logService.InsertAsync(new SysLog
            {
                LogId = PubId.SnowflakeId,
                Browser = GetBrowser(),
                CreateBy = userId,
                CreateDate = DateTime.UtcNow,
                Description = userId + "用户删除角色" + role.RoleName,
                LogIp = GetIp(),
                Url = GetUrl(),
                LogType = LogType.delete.ToString()
            });
            return new JsonResult((flag, PubConst.Delete1));

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Role/DisableRole")]
        public async Task<IActionResult> DisableRole(string token, long userId, SysRole role)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            bool flag = false;
            flag = await _roleService.DisableRole(role, userId);
            await _logService.InsertAsync(new SysLog
            {
                LogId = PubId.SnowflakeId,
                Browser = GetBrowser(),
                CreateBy = userId,
                CreateDate = DateTime.UtcNow,
                Description = userId + "用户禁用角色" + role.RoleName,
                LogIp = GetIp(),
                Url = GetUrl(),
                LogType = LogType.disable.ToString()
            });
            return new JsonResult((flag, PubConst.Enable3));

        }

        
    }
}

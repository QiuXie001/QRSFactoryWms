using DB.Dto;
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
        public async Task<string> GetPageList([FromForm] string bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return (false, PubConst.ValidateToken2).ToJson();
            }

            var bootstrapDto = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);

            if (bootstrapDto == null || bootstrapDto._ == null)
                bootstrapDto = PubConst.DefaultBootstrapParams;

            var item = await _roleService.PageListAsync(bootstrapDto);
            return item;
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("Role/GetRoleList")]
        public async Task<IActionResult> GetRoleList(string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var item = await _roleService.GetRoleList();
            return Ok(item);
        }
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("Role/GetMenuByRoleId")]
        public async Task<IActionResult> GetMenuByRoleId(long roleId, long userId, string token)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var roleMenu = await _roleService.GetMenuAsync(roleId);
            return Ok(roleMenu);
        }
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.add)]
        [Route("Role/InsertRole")]
        public async Task<IActionResult> InsertRole(string token, long userId, [FromForm] string role, [FromForm] string menuId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var roleDto = JsonConvert.DeserializeObject<RoleDto>(role);
            if (menuId == null)
                menuId = "";
            var menuIds = menuId.Split(',').ToArray();
            bool flag = false;
            SysRole entity = new SysRole()
            {
                RoleName = roleDto.RoleName,
                RoleType = roleDto.RoleType,
                Remark = roleDto.Remark,
            };
            flag = await _roleService.InsertRole(entity, userId, menuIds);
            if (flag)
            {
                return Ok((flag, PubConst.Add1));
            }
            return Ok((flag, PubConst.Add2));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("Role/UpdateRole")]
        public async Task<IActionResult> UpdateRole(string token, long userId, [FromForm] string role, [FromForm] string menuId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var roleDto = JsonConvert.DeserializeObject<RoleDto>(role);
            var menuIds = menuId.Split(',').ToArray();
            bool flag = false;
            flag = await _roleService.UpdateRole(roleDto, userId, menuIds);
            return Ok((flag, PubConst.Update1));

        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Role/DeleteRole")]
        public async Task<IActionResult> DeleteRole(string token, long userId, [FromForm] string role)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var roleDto = JsonConvert.DeserializeObject<SysRole>(role);
            bool flag = false;
            flag = await _roleService.DeleteRole(roleDto);
            return Ok((flag, PubConst.Delete1));

        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.disable)]
        [Route("Role/DisableRole")]
        public async Task<IActionResult> DisableRole(string token, long userId, [FromForm] string role)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var roleDto = JsonConvert.DeserializeObject<SysRole>(role);
            bool flag = false;
            flag = await _roleService.DisableRole(roleDto, userId);
            return Ok((flag, PubConst.Enable3));

        }


    }
}

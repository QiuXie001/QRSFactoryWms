using DB.Models;
using IServices.Sys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using System.Security.Claims;

namespace QRSFactoryWmsAPI.Controllers.Sys
{
    public class MenuController : BaseController
    {
        private readonly IMemoryCache _cache;
        private readonly ISys_RoleService _roleService;
        private readonly ISys_IdentityService _identityService;
        private readonly ISys_LogService _logService;
        private readonly string NowUrl = "/Menu";

        public MenuController(
            IMemoryCache cache, 
            ISys_RoleService roleService, 
            ISys_LogService logService,
            ISys_IdentityService identityService)
        {
            _cache = cache;
            _roleService = roleService;
            _logService = logService;
            _identityService = identityService;
        }

        [HttpGet]
        [EnableCors]
        [AllowAnonymous]
        [Route("Menu/GetPageList")]
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
                Description = userId + "用户获取菜单分页列表",
                LogIp = GetIp(),
                Url = GetUrl(),
                LogType = LogType.getList.ToString()
            });
            return item;
        }
        [HttpGet]
        [EnableCors]
        [AllowAnonymous]
        [Route("Menu/InsertMenu")]
        public async Task<IActionResult> InsertMenu(string token, long userId, SysMenu menu)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            bool flag = false;
            flag = await _roleService.InsertMenu(menu, userId);
            await _logService.InsertAsync(new SysLog
            {
                LogId = PubId.SnowflakeId,
                Browser = GetBrowser(),
                CreateBy = userId,
                CreateDate = DateTime.UtcNow,
                Description = userId + "用户新增新菜单" + menu.MenuName,
                LogIp = GetIp(),
                Url = GetUrl(),
                LogType = LogType.add.ToString()
            });
            return new JsonResult((flag, PubConst.Add1));
        }

        [HttpGet]
        [EnableCors]
        [AllowAnonymous]
        [Route("Menu/UpdateMenu")]
        public async Task<IActionResult> UpdateMenu(string token, long userId, SysMenu menu)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            bool flag = false;
            flag = await _roleService.UpdateMenu(menu, userId);
            await _logService.InsertAsync(new SysLog
            {
                LogId = PubId.SnowflakeId,
                Browser = GetBrowser(),
                CreateBy = userId,
                CreateDate = DateTime.UtcNow,
                Description = userId + "用户修改菜单" + menu.MenuName,
                LogIp = GetIp(),
                Url = GetUrl(),
                LogType = LogType.update.ToString()
            });
            return new JsonResult((flag, PubConst.Update1));
        }

        [HttpGet]
        [EnableCors]
        [AllowAnonymous]
        [Route("Menu/DeleteMenu")]
        public async Task<IActionResult> DeleteMenu(string token, long userId, SysMenu menu)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            bool flag = false;
            flag = await _roleService.DeleteMenu(menu);
            await _logService.InsertAsync(new SysLog
            {
                LogId = PubId.SnowflakeId,
                Browser = GetBrowser(),
                CreateBy = userId,
                CreateDate = DateTime.UtcNow,
                Description = userId + "用户删除菜单" + menu.MenuName,
                LogIp = GetIp(),
                Url = GetUrl(),
                LogType = LogType.delete.ToString()
            });
            return new JsonResult((flag, PubConst.Delete1));
        }

        [HttpGet]
        [EnableCors]
        [AllowAnonymous]
        [Route("Menu/DisableMenu")]
        public async Task<IActionResult> DisableMenu(string token, long userId, SysMenu menu)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            bool flag = false;
            flag = await _roleService.DisableMenu(menu, userId);
            await _logService.InsertAsync(new SysLog
            {
                LogId = PubId.SnowflakeId,
                Browser = GetBrowser(),
                CreateBy = userId,
                CreateDate = DateTime.UtcNow,
                Description = userId + "用户禁用菜单" + menu.MenuName,
                LogIp = GetIp(),
                Url = GetUrl(),
                LogType = LogType.disable.ToString()
            });
            return new JsonResult((flag, PubConst.Enable3));
        }
    }
}

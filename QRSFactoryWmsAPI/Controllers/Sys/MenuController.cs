using DB.Models;
using IServices.Sys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;

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

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("Menu/GetPageList")]
        public async Task<string> GetPageList([FromForm] string bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return (false, PubConst.ValidateToken2).ToJson();
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var item = await _roleService.GetMenuAsync(bootstrapObject);
            return item;
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("Menu/GetMenus")]
        public async Task<IActionResult> GetMenus(string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return NotFound((false, PubConst.ValidateToken2));
            }
            var item = await _roleService.GetMenuAsync();
            return Ok(item);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.add)]
        [Route("Menu/InsertMenu")]
        public async Task<IActionResult> InsertMenu(string token, long userId, [FromForm] string menu)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }

            var menuObject = JsonConvert.DeserializeObject<SysMenu>(menu);
            bool flag = false;
            flag = await _roleService.InsertMenu(menuObject, userId);
            return new JsonResult((flag, PubConst.Add1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("Menu/UpdateMenu")]
        public async Task<IActionResult> UpdateMenu(string token, long userId, [FromForm] string menu)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var menuObject = JsonConvert.DeserializeObject<SysMenu>(menu);
            bool flag = false;
            flag = await _roleService.UpdateMenu(menuObject, userId);
            return new JsonResult((flag, PubConst.Update1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Menu/DeleteMenu")]
        public async Task<IActionResult> DeleteMenu(string token, long userId, [FromForm] string menu)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var menuObject = JsonConvert.DeserializeObject<SysMenu>(menu);
            bool flag = false;
            flag = await _roleService.DeleteMenu(menuObject);
            return new JsonResult((flag, PubConst.Delete1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.disable)]
        [Route("Menu/DisableMenu")]
        public async Task<IActionResult> DisableMenu(string token, long userId, [FromForm] string menu)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var menuObject = JsonConvert.DeserializeObject<SysMenu>(menu);
            bool flag = false;
            flag = await _roleService.DisableMenu(menuObject, userId);
            return new JsonResult((flag, PubConst.Enable3));
        }
    }
}

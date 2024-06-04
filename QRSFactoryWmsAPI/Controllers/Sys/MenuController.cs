using DB.Models;
using IServices.Sys;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using System.Security.Claims;

namespace QRSFactoryWmsAPI.Controllers.Sys
{
    public class MenuController : BaseController
    {
        private readonly IMemoryCache _cache;
        private readonly ISys_MenuService _menuServices;

        public MenuController(IMemoryCache cache, ISys_MenuService menuServices)
        {
            _cache = cache;
            _menuServices = menuServices;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Menu/GetPageList")]
        public async Task<string> GetPageList(Bootstrap.BootstrapParams bootstrap)
        {

            if (bootstrap._ == null)
                bootstrap = PubConst.DefaultBootstrapParams;
            var item = await _menuServices.PageListAsync(bootstrap);

            return item;
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("Menu/Insert")]
        public async Task<IActionResult> Insert(Bootstrap.BootstrapParams bootstrap,SysMenu menu)
        {

            var item = await _menuServices.InsertAsync(menu);

            return new JsonResult((item, PubConst.Add1));
        }
    }
}

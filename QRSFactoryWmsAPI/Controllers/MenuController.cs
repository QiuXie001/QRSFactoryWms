using IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace QRSFactoryWmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly ISys_RoleService _roleServices;

        public MenuController(IMemoryCache cache, ISys_RoleService roleServices)
        {
            _cache = cache;
            _roleServices = roleServices;
        }

        [HttpGet("menu")]
        public string GetMenu()
        {
            // 获取用户信息，获取菜单数据等逻辑
            var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            var menu = _cache.Get("menu_" + userId) ?? _roleServices.GetMenu(long.Parse(userId));
            return menu.ToString();
        }

    }
}

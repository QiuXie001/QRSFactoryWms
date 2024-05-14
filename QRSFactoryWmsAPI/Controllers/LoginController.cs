using IServices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qiu.Utils.Security;
using Qiu.Utils.Table;

namespace QRSFactoryWmsAPI.Controllers
{
    public class LoginController : Controller
    {
        private readonly ISys_UserServices _userServices;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ISys_LogServices _logServices;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;

        public LoginController(Xss xss, ISys_LogServices logServices, IHttpContextAccessor httpContext, IConfiguration configuration, ISys_UserServices sys_User, IMediator mediator)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _userServices = sys_User;
            _logServices = logServices;
            _xss = xss;
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Login/GetAllUser")]
        public async Task<string> GetAllUser()
        {
            var bootstrap = new Bootstrap.BootstrapParams();
            bootstrap.offset = 0; // 设置为0以获取第一页的数据
            bootstrap.limit = 20; // 设置为足够大的值以获取整个列表
            bootstrap.search = string.Empty; // 设置搜索参数为空字符串以获取整个列表
            bootstrap.datemin = string.Empty;
            bootstrap.datemax = "2024-05-31";
            bootstrap.order = "desc";
            bootstrap.sort = "CreateDate";

            var item = await _userServices.PageList(bootstrap);

            return item;
        }
    }
}

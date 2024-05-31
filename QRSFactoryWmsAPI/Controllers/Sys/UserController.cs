using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Security;
using Qiu.Utils.Table;

namespace QRSFactoryWmsAPI.Controllers
{
    public class UserController : BaseController
    {
        private readonly ISys_UserService _userServices;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ISys_LogService _logService;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;

        public UserController(Xss xss, ISys_LogService logService, IHttpContextAccessor httpContext, IConfiguration configuration, ISys_UserService userServices, IMediator mediator)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _userServices = userServices;
            _logService = logService;
            _xss = xss;
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("User/GetUsers")]
        public async Task<string> GetUsers()
        {
            var bootstrap = new Bootstrap.BootstrapParams();
            bootstrap.offset = 0; // 设置为0以获取第一页的数据
            bootstrap.limit = 20; // 设置为足够大的值以获取整个列表
            bootstrap.search = string.Empty; // 设置搜索参数为空字符串以获取整个列表
            bootstrap.datemin = string.Empty;
            bootstrap.datemax = "2024-05-31";
            bootstrap.order = "desc";
            bootstrap.sort = "CreateDate";

            var item = await _userServices.PageListAsync(bootstrap);

            return item;
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("User/GetPageList")]
        public async Task<string> GetPageList(Bootstrap.BootstrapParams bootstrap)
        {

            var item = await _userServices.PageListAsync(bootstrap);

            return item;
        }
    }
}

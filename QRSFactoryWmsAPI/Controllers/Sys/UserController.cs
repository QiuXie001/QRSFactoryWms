using DB.Models;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
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

        public UserController(Xss xss, 
            ISys_LogService logService, 
            IHttpContextAccessor httpContext, 
            IConfiguration configuration, 
            ISys_UserService userServices, 
            IMediator mediator)
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
            var bootstrap = PubConst.DefaultBootstrapParams;

            var item = await _userServices.PageListAsync(bootstrap);

            return item;
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("User/GetPageList")]
        public async Task<string> GetPageList(Bootstrap.BootstrapParams bootstrap)
        {
            if(bootstrap._ == null)
                bootstrap =PubConst.DefaultBootstrapParams;
            var item = await _userServices.PageListAsync(bootstrap);

            return item;
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("User/Insert")]
        public async Task<IActionResult> Insert(SysUser user)
        {

            var item = await _userServices.InsertAsync(user);

            return new JsonResult((item, PubConst.Add1));
        }
    }
}

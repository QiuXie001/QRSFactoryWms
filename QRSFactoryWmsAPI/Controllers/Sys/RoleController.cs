using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Security;
using Qiu.Utils.Table;

namespace QRSFactoryWmsAPI.Controllers.Sys
{
    public class RoleController : BaseController
    {
        private readonly ISys_RoleService _roleServices;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ISys_LogService _logService;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;

        public RoleController(Xss xss, ISys_LogService logService, IHttpContextAccessor httpContext, IConfiguration configuration, ISys_RoleService roleServices, IMediator mediator)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _roleServices = roleServices;
            _logService = logService;
            _xss = xss;
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Role/GetPageList")]
        public async Task<string> GetPageList(Bootstrap.BootstrapParams bootstrap)
        {

            var item = await _roleServices.PageListAsync(bootstrap);
            return item;
        }
    }
}

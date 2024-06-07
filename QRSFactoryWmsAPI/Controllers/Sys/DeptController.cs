using DB.Models;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Security;
using Qiu.Utils.Table;
using Services.Sys;

namespace QRSFactoryWmsAPI.Controllers.Sys
{
    public class DeptController : BaseController
    {
        private readonly ISys_DeptService _deptService;
        private readonly ISys_IdentityService _identityService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;
        private readonly string NowUrl = "/Dept";
        public DeptController(Xss xss,
            ISys_DeptService deptService,
            ISys_IdentityService identityService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            IMediator mediator)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _deptService = deptService;
            _identityService = identityService;
            _xss = xss;
            _mediator = mediator;
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("Dept/GetPageList")]
        public async Task<string> GetPageList(Bootstrap.BootstrapParams bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return (false, PubConst.ValidateToken2).ToJson();
            }
            if (bootstrap._ == null)
                bootstrap = PubConst.DefaultBootstrapParams;
            var item = await _deptService.PageListAsync(bootstrap);
            return item;
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.add)]
        [Route("Dept/Insert")]
        public async Task<IActionResult> Insert(SysDept dept, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var item = await _deptService.InsertAsync(dept);
            return new JsonResult((item, PubConst.Add1));
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("Dept/Update")]
        public async Task<IActionResult> Update(SysDept dept, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var item = await _deptService.InsertAsync(dept);
            return new JsonResult((item, PubConst.Add1));
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Dept/Delete")]
        public async Task<IActionResult> Delete(SysDept dept, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var item = await _deptService.InsertAsync(dept);
            return new JsonResult((item, PubConst.Add1));
        }
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.disable)]
        [Route("Dept/Disable")]
        public async Task<IActionResult> Disable(SysDept dept, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var item = await _deptService.InsertAsync(dept);
            return new JsonResult((item, PubConst.Add1));
        }
    }
}

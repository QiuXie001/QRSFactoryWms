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
    public class DictController : BaseController
    {
        private readonly ISys_DictService _dictService;
        private readonly ISys_IdentityService _identityService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;
        private readonly string NowUrl = "/Dict";
        public DictController(Xss xss,
            ISys_DictService dictService,
            ISys_IdentityService identityService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            IMediator mediator)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _dictService = dictService;
            _identityService = identityService;
            _xss = xss;
            _mediator = mediator;
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("Dict/GetPageList")]
        public async Task<string> GetPageList(Bootstrap.BootstrapParams bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return (false, PubConst.ValidateToken2).ToJson();
            }
            if (bootstrap._ == null)
                bootstrap = PubConst.DefaultBootstrapParams;
            var item = await _dictService.PageListAsync(bootstrap);
            return item;
        }
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.add)]
        [Route("Dict/Insert")]
        public async Task<IActionResult> Insert(SysDict dict, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var item = await _dictService.InsertAsync(dict);
            return new JsonResult((item, PubConst.Add1));
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("Dict/Update")]
        public async Task<IActionResult> Update(SysDict dict, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var item = await _dictService.InsertAsync(dict);
            return new JsonResult((item, PubConst.Add1));
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Dict/Delete")]
        public async Task<IActionResult> Delete(SysDict dict, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var item = await _dictService.InsertAsync(dict);
            return new JsonResult((item, PubConst.Add1));
        }
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.disable)]
        [Route("Dict/Disable")]
        public async Task<IActionResult> Disable(SysDict dict, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var item = await _dictService.InsertAsync(dict);
            return new JsonResult((item, PubConst.Add1));
        }
    }
}

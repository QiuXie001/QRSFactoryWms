using DB.Models;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
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

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("Dept/GetPageList")]
        public async Task<string> GetPageList([FromForm] string bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return (false, PubConst.ValidateToken2).ToJson();
            }

            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var item = await _deptService.PageListAsync(bootstrapObject);
            return item;
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.add)]
        [Route("Dept/Insert")]
        public async Task<IActionResult> Insert([FromForm] string dept, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }

            var deptObject = JsonConvert.DeserializeObject<SysDept>(dept);
            var item = await _deptService.InsertAsync(deptObject);
            return new JsonResult((item, PubConst.Add1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("Dept/Update")]
        public async Task<IActionResult> Update([FromForm] string dept, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }

            var deptObject = JsonConvert.DeserializeObject<SysDept>(dept);
            var item = await _deptService.UpdateAsync(deptObject);
            return new JsonResult((item, PubConst.Update1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Dept/Delete")]
        public async Task<IActionResult> Delete([FromForm] string dept, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }

            var deptObject = JsonConvert.DeserializeObject<SysDept>(dept);
            var item = await _deptService.DeleteAsync(deptObject);
            return new JsonResult((item, PubConst.Delete1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.disable)]
        [Route("Dept/Disable")]
        public async Task<IActionResult> Disable([FromForm] string dept, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }

            var deptObject = JsonConvert.DeserializeObject<SysDept>(dept);
            deptObject.IsDel = 0;
            deptObject.ModifiedBy = userId;
            deptObject.ModifiedDate = DateTime.Now;
            var item = await _deptService.UpdateAsync(deptObject);
            return new JsonResult((item, PubConst.Update2));
        }
    }
}

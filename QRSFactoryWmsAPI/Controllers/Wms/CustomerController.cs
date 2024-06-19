using DB.Models;
using IServices;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Extensions;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using SqlSugar;
using System.IO;
using System.Linq;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Delegate;
using Qiu.Utils.Excel;
using Qiu.Utils.Extensions;
using Qiu.Utils.Files;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IServices.Wms;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace KopSoftWms.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IWms_CustomerService _customerService;
        private readonly ISys_IdentityService _identityService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly string NowUrl = "/Customer";

        public CustomerController(IWms_CustomerService customerService,
            ISys_IdentityService identityService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            IMediator mediator)
        {
            _customerService = customerService;
            _identityService = identityService;
            _httpContext = httpContext;
            _configuration = configuration;
            _mediator = mediator;
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("Customer/List")]
        public async Task<IActionResult> ListAsync([FromForm] string bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var sd = await _customerService.PageListAsync(bootstrapObject);
            return new JsonResult(sd);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.add)]
        [Route("Customer/Add")]
        public async Task<IActionResult> AddAsync(string token, long userId, WmsCustomer model)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            if (model.IsEmpty())
            {
                return new JsonResult(false, PubConst.Null);
            }
            if (await _customerService.IsAnyAsync(c => c.CustomerNo == model.CustomerNo || c.CustomerName == model.CustomerName))
            {
                return new JsonResult((false, PubConst.Customer1));
            }
            model.CustomerId = PubId.SnowflakeId;
            model.CreateBy = UserDtoCache.UserId;
            model.CreateDate = DateTime.UtcNow;
            model.ModifiedBy = UserDtoCache.UserId;
            model.ModifiedDate = DateTime.UtcNow;
            bool flag = await _customerService.InsertAsync(model);
            return new JsonResult((flag, PubConst.Add1));

        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("Customer/Update")]
        public async Task<IActionResult> UpdateAsync(WmsCustomer model, string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            if (model.IsEmpty())
            {
                return new JsonResult(false, PubConst.Null);
            }
            model.CustomerId = Id;
            model.ModifiedBy = UserDtoCache.UserId;
            model.ModifiedDate = DateTime.UtcNow;
            bool flag = await _customerService.UpdateAsync(model);
            return new JsonResult((flag, PubConst.Update1));

        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Customer/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var flag = await _customerService.UpdateAsync(new WmsCustomer { CustomerId = Id, IsDel = 1, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTime.UtcNow }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            return new JsonResult((flag, PubConst.Delete1));
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [Route("Customer/Download")]
        public Task<IActionResult> Download()
        {
            var stream = System.IO.File.OpenRead(Path.Combine(WebRoot, "excel", "customer.xlsx"));
            return Task.FromResult<IActionResult>(File(stream, ContentType.ContentTypeExcel, "customer.xlsx"));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [Route("Customer/ImportExcel")]
        public async Task<IActionResult> ImportExcelAsync(string token, long userId, IFormFile file)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            if (file == null || file.Length <= 0)
            {
                return new JsonResult((false, PubConst.File1));
            }
            string fileExt = Path.GetExtension(file.FileName).ToLower();
            if (!NpoiUtil.excel.Contains(fileExt))
            {
                return new JsonResult((false, PubConst.File2));
            }
            var filepath = Path.Combine(WebRoot, "upload", PubId.GetUuid()) + fileExt;
            //1 直接通过流
            return DelegateUtil.TryExecute<IActionResult>(() =>
            {
                using (var st = new MemoryStream())
                {
                    file.CopyTo(st);
                    var dt = NpoiUtil.Import(st, fileExt);
                    var json = _customerService.ImportAsync(dt, UserDtoCache.UserId).JilToJson();
                    return new JsonResult(json);
                }
            }, new JsonResult((false, PubConst.File3))
             );
        }
    }
}

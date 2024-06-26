using DB.Dto;
using DB.Models;
using IServices.Sys;
using IServices.Wms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Delegate;
using Qiu.Utils.Excel;
using Qiu.Utils.Extensions;
using Qiu.Utils.Files;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;

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
        [Route("Customer/GetCustomerList")]
        public async Task<IActionResult> GetCustomerListAsync( string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var sd = await _customerService.GetCustomerList();
            return Ok(sd);
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
                return Ok((false, PubConst.ValidateToken2));
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var sd = await _customerService.PageListAsync(bootstrapObject);
            return Ok(sd);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.add)]
        [Route("Customer/Add")]
        public async Task<IActionResult> AddAsync(string token, long userId, [FromForm] string model)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            if (model.IsEmpty())
            {
                return Ok((false, PubConst.Null));
            }
            var modelObject = JsonConvert.DeserializeObject<WmsCustomer>(model);
            if (await _customerService.IsAnyAsync(c => c.CustomerNo == modelObject.CustomerNo || c.CustomerName == modelObject.CustomerName))
            {
                return Ok((false, PubConst.Customer1));
            }
            modelObject.CustomerId = PubId.SnowflakeId;
            modelObject.IsDel = 1;
            modelObject.CreateBy = userId;
            modelObject.CreateDate = DateTime.UtcNow;
            modelObject.ModifiedBy = userId;
            modelObject.ModifiedDate = DateTime.UtcNow;
            bool flag = await _customerService.InsertAsync(modelObject);
            return Ok((flag, PubConst.Add1));

        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("Customer/Update")]
        public async Task<IActionResult> UpdateAsync([FromForm] string model, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            if (model.IsEmpty())
            {
                return Ok((false, PubConst.Null));
            }
            var modelObject = JsonConvert.DeserializeObject<CustomerDto>(model);
            var entity = await _customerService.QueryableToSingleAsync(c => c.CustomerId == modelObject.CustomerId&&c.IsDel==1);
            entity.CustomerNo = modelObject.CustomerNo;
            entity.CustomerName = modelObject.CustomerName;
            entity.Address = modelObject.Address;
            entity.Tel = modelObject.Tel;
            entity.Email = modelObject.Email;
            entity.CustomerLevel = modelObject.CustomerLevel;
            entity.CustomerPerson = modelObject.CustomerPerson;
            entity.Remark = modelObject.Remark;

            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.UtcNow;
            bool flag = await _customerService.UpdateAsync(entity);
            return Ok((flag, PubConst.Update1));

        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Customer/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var flag = await _customerService.UpdateAsync(new WmsCustomer { CustomerId = Id, IsDel = 1, ModifiedBy = userId, ModifiedDate = DateTime.UtcNow }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            return Ok((flag, PubConst.Delete1));
        }

        [HttpPost]
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
                return Ok((false, PubConst.ValidateToken2));
            }
            if (file == null || file.Length <= 0)
            {
                return Ok((false, PubConst.File1));
            }
            string fileExt = Path.GetExtension(file.FileName).ToLower();
            if (!NpoiUtil.excel.Contains(fileExt))
            {
                return Ok((false, PubConst.File2));
            }
            var filepath = Path.Combine(WebRoot, "upload", PubId.GetUuid()) + fileExt;
            //1 直接通过流
            return DelegateUtil.TryExecute<IActionResult>(() =>
            {
                using (var st = new MemoryStream())
                {
                    file.CopyTo(st);
                    var dt = NpoiUtil.Import(st, fileExt);
                    var json = _customerService.ImportAsync(dt, userId).JilToJson();
                    return Ok(json);
                }
            }, Ok((false, PubConst.File3))
             );
        }
    }
}

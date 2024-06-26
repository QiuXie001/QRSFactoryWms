using AngleSharp.Dom;
using DB.Dto;
using DB.Models;
using IServices.Sys;
using IServices.Wms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qiu.Core.Dto;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Extensions;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using SqlSugar;

namespace QRSFactoryWmsAPI.Controllers.Wms
{
    public class StockOutController : BaseController
    {
        private readonly ISys_DictService _dictService;
        private readonly IWms_CustomerService _customerService;
        private readonly IWms_StockoutService _stockoutService;
        private readonly ISys_SerialnumService _serialnumService;
        private readonly IWms_StockoutdetailService _stockoutdetailService;
        private readonly IWms_InventoryService _inventoryService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly ISys_IdentityService _identityService;
        private readonly string NowUrl = "/StockOut";

        public StockOutController(ISys_DictService dictService,
            IWms_CustomerService customerService,
            IWms_StockoutService stockoutService,
            ISys_SerialnumService serialnumService,
            IWms_StockoutdetailService stockoutdetailService,
            IWms_InventoryService inventoryService,
            IMediator mediator,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            ISys_IdentityService identityService)
        {
            _dictService = dictService;
            _customerService = customerService;
            _stockoutService = stockoutService;
            _serialnumService = serialnumService;
            _stockoutdetailService = stockoutdetailService;
            _inventoryService = inventoryService;
            _mediator = mediator;
            _httpContext = httpContext;
            _configuration = configuration;
            _identityService = identityService;
        }


        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("StockOut/Search")]
        public async Task<IActionResult> SearchAsync(string token, long userId, string text)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var bootstrap = new Bootstrap.BootstrapParams
            {
                limit = 100,
                offset = 0,
                sort = "CreateDate",
                search = text,
                order = "desc"
            };
            var json = await _customerService.PageListAsync(bootstrap);
            return Content(json);
        }
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("StockOut/List")]
        public async Task<IActionResult> ListAsync([FromForm] string bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var bootstrapObject = JsonConvert.DeserializeObject<PubParams.StockOutBootstrapParams>(bootstrap);
            var sd = await _stockoutService.PageListAsync(bootstrapObject);
            return Ok(sd);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [Route("StockOut/SearchInventory")]
        public async Task<IActionResult> SearchInventoryAsync(string token, long userId, long Id, string storagerackId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var bootstrap = new PubParams.InventoryBootstrapParams
            {
                limit = 100,
                offset = 0,
                sort = "CreateDate",
                search = Id.ToString(),
                order = "desc",
                StorageRackId = storagerackId,
            };
            var json = await _inventoryService.SearchInventoryAsync(bootstrap);
            return Content(json);
        }
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("StockOut/ListDetail")]
        public async Task<IActionResult> ListDetailAsync(string token, long userId, string pid)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var sd = await _stockoutdetailService.PageListAsync(pid);
            return Ok(sd);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("StockOut/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync([FromForm] string model, long Id, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            if (Id == 0)
            {
                var modelObject = JsonConvert.DeserializeObject<WmsStockout>(model);
                if (!modelObject.OrderNo.IsEmpty())
                {
                    if (await _stockoutService.IsAnyAsync(c => c.OrderNo == modelObject.OrderNo))
                    {
                        return Ok((false, PubConst.StockIn1));
                    }
                }
                modelObject.StockOutId = PubId.SnowflakeId;
                modelObject.StockOutNo = modelObject.StockOutNo;
                modelObject.OrderNo = modelObject.OrderNo;
                modelObject.StockOutTypeId = modelObject.StockOutTypeId;
                modelObject.CustomerId = modelObject.CustomerId;
                modelObject.Remark = modelObject.Remark;

                modelObject.IsDel = 1;
                modelObject.StockOutStatus = StockInStatus.initial.ToByte();
                modelObject.CreateBy = userId;
                modelObject.CreateDate = DateTime.UtcNow;
                modelObject.ModifiedBy = userId;
                modelObject.ModifiedDate = DateTime.UtcNow;
                bool flag = await _stockoutService.InsertAsync(modelObject);
                return Ok((flag, flag ? PubConst.Add1 : PubConst.Add2));
            }
            else
            {
                var modelObject = JsonConvert.DeserializeObject<StockOutDto>(model);
                var entity = await _stockoutService.QueryableToSingleAsync(so => so.StockOutId == modelObject.StockOutId && so.IsDel == 1);
                entity.OrderNo = modelObject.OrderNo;
                entity.StockOutNo = modelObject.StockOutNo;
                entity.StockOutTypeId = modelObject.StockOutTypeId;
                entity.CustomerId = modelObject.CustomerId;
                entity.StockOutStatus = StockInStatus.initial.ToByte();
                entity.Remark = modelObject.Remark;
                entity.ModifiedBy = userId;
                entity.ModifiedDate = DateTime.UtcNow;
                var flag = await _stockoutService.UpdateAsync(entity);
                return Ok((flag, flag ? PubConst.Update1 : PubConst.Update2));
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("StockOut/AddOrUpdateDetail")]
        public async Task<IActionResult> AddOrUpdateDetailAsync([FromForm] string model, long Id, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            if (Id.IsZero())
            {
                var modelObject = JsonConvert.DeserializeObject<WmsStockoutdetail>(model);
                modelObject.StockOutDetailId = PubId.SnowflakeId;

                modelObject.StoragerackId = 1;
                modelObject.IsDel = 1;
                modelObject.Status = StockInStatus.initial.ToByte();
                modelObject.AuditinId = userId;
                modelObject.AuditinTime = DateTime.UtcNow;
                modelObject.CreateBy = userId;
                modelObject.CreateDate = DateTime.UtcNow;
                modelObject.ModifiedBy = userId;
                modelObject.ModifiedDate = DateTime.UtcNow;
                bool flag = await _stockoutdetailService.InsertAsync(modelObject);
                return Ok((flag, flag ? PubConst.Add1 : PubConst.Add2));
            }
            else
            {
                var modelObject = JsonConvert.DeserializeObject<WmsStockoutdetail>(model);
                //modelObject.StockOutDetailId = Id;
                //modelObject.ModifiedBy = userId;
                //modelObject.ModifiedDate = DateTimeExt.DateTime;
                var flag = await _stockoutdetailService.UpdateAsync(modelObject);
                return Ok((flag, flag ? PubConst.Update1 : PubConst.Update2));
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("StockOut/Auditin")]
        public async Task<IActionResult> AuditinAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var list = await _stockoutdetailService.QueryableToListAsync(c => c.IsDel == 1 && c.StockOutId == Id);
            if (!list.Any())
            {
                return Ok((false, PubConst.StockIn4));
            }
            var flag = await _stockoutService.AuditinAsync(userId, Id);
            return Ok((flag, flag ? PubConst.StockIn2 : PubConst.StockIn3));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("StockOut/Auditin")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var isExist = await _stockoutdetailService.IsAnyAsync(c => c.StockOutId == Id);
            if (isExist)
            {
                return Ok((false, PubConst.StockIn3));
            }
            else
            {
                var flag1 = await _stockoutdetailService.UpdateAsync(
                     new WmsStockoutdetail { IsDel = 0, ModifiedBy = userId, ModifiedDate = DateTimeExt.DateTime },
                     c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate },
                     c => c.StockOutId == Id
                 );

                var flag2 = await _stockoutService.UpdateAsync(
                    new WmsStockout { IsDel = 0, ModifiedBy = userId, ModifiedDate = DateTimeExt.DateTime },
                    c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate },
                    c => c.StockOutId == Id
                );

                return Ok(new { Success = flag1 && flag2, Message = flag1 && flag2 ? PubConst.Delete1 : PubConst.Delete2 });
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("StockOut/DeleteDetail")]
        public async Task<IActionResult> DeleteDetailAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var flag = await _stockoutdetailService.UpdateAsync(
                 new WmsStockoutdetail { IsDel = 0, ModifiedBy = userId, ModifiedDate = DateTimeExt.DateTime },
                 c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate },
                 c => c.StockOutDetailId == SqlFunc.ToInt64(Id)
                 );
            return BootJsonH(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        }


        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("StockOut/PreviewJson")]
        public async Task<IActionResult> PreviewJsonAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var str = _stockoutService.PrintList(Id);
            return Content(str);
        }




    }
}

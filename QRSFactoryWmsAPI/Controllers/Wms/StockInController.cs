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

namespace QRSFactoryWmsAPI.Controllers.Wms
{
    public class StockInController : BaseController
    {
        private readonly IWms_StockindetailService _stockindetailService;
        private readonly ISys_SerialnumService _serialnumService;
        private readonly ISys_DictService _dictService;
        private readonly IWms_SupplierService _supplierService;
        private readonly IWms_StockinService _stockinService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly ISys_IdentityService _identityService;
        private readonly string NowUrl = "/StockIn";

        public StockInController(IWms_StockindetailService stockindetailService,
            ISys_SerialnumService serialnumService,
            ISys_DictService dictService,
            IWms_SupplierService supplierService,
            IWms_StockinService stockinService,
            IMediator mediator,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            ISys_IdentityService identityService)
        {
            _stockindetailService = stockindetailService;
            _serialnumService = serialnumService;
            _dictService = dictService;
            _supplierService = supplierService;
            _stockinService = stockinService;
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
        [Route("StockIn/List")]
        public async Task<IActionResult> ListAsync(string token, long userId, [FromForm] string bootstrap)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var bootstrapObject = JsonConvert.DeserializeObject<PubParams.StockInBootstrapParams>(bootstrap);

            var sd = await _stockinService.PageListAsync(bootstrapObject);
            return new JsonResult(sd);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("StockIn/ListDetail")]
        public async Task<IActionResult> ListDetailAsync(string token, long userId, string pid)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var sd = await _stockindetailService.PageListAsync(pid);
            return new JsonResult(sd);
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [Route("StockIn/Detail")]
        public async Task<IActionResult> Detail(string token, long userId, long id, string pid)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var model = new WmsStockindetail();
            if (id.IsZero())
            {
                model.StockInId = pid.ToInt64();
                return View(model);
            }
            else
            {
                model = await _stockindetailService.QueryableToSingleAsync(c => c.StockInDetailId == id && c.IsDel == 1);
                return View(model);
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("StockIn/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync(string token, long userId, [FromForm] string model, string id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var modelObject = JsonConvert.DeserializeObject<WmsStockin>(model);
            if (id.IsEmptyZero())
            {
                if (!modelObject.OrderNo.IsEmpty())
                {
                    if (await _stockinService.IsAnyAsync(c => c.OrderNo == modelObject.OrderNo))
                    {
                        return new JsonResult((false, PubConst.StockIn1));
                    }
                }
                modelObject.StockInNo = await _serialnumService.GetSerialnumAsync(UserDtoCache.UserId, "Wms_stockin");
                modelObject.StockInId = PubId.SnowflakeId;
                modelObject.StockInStatus = StockInStatus.initial.ToByte();
                modelObject.CreateBy = UserDtoCache.UserId;
                bool flag = await _stockinService.InsertAsync(modelObject);
                return new JsonResult((flag, PubConst.Add1));
            }
            else
            {
                modelObject.StockInId = id.ToInt64();
                modelObject.ModifiedBy = UserDtoCache.UserId;
                modelObject.ModifiedDate = DateTimeExt.DateTime;
                var flag = await _stockinService.UpdateAsync(modelObject);
                return new JsonResult((flag, PubConst.Update1));
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("StockIn/AddOrUpdateD")]
        public async Task<IActionResult> AddOrUpdateDAsync(string token, long userId, [FromForm] string model, string id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var modelObject = JsonConvert.DeserializeObject<WmsStockindetail>(model);
            if (id.IsEmptyZero())
            {
                modelObject.StockInDetailId = PubId.SnowflakeId;
                modelObject.Status = StockInStatus.initial.ToByte();
                modelObject.CreateBy = UserDtoCache.UserId;
                bool flag = await _stockindetailService.InsertAsync(modelObject);
                return new JsonResult((flag, PubConst.Add1));
            }
            else
            {
                modelObject.StockInDetailId = id.ToInt64();
                modelObject.ModifiedBy = UserDtoCache.UserId;
                modelObject.ModifiedDate = DateTimeExt.DateTime;
                var flag = await _stockindetailService.UpdateAsync(modelObject);
                return new JsonResult((flag, PubConst.Update1));
            }
        }

        [HttpGet]
        [OperationLog(LogType.update)]
        public async Task<IActionResult> AuditinAsync(string token, long userId, long id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var list = await _stockindetailService.QueryableToListAsync(c => c.IsDel == 1 && c.StockInId == id);
            if (!list.Any())
            {
                return new JsonResult((false, PubConst.StockIn4));
            }
            var flag = await _stockinService.AuditinAsync(UserDtoCache.UserId, id);
            return new JsonResult((flag, flag ? PubConst.StockIn2 : PubConst.StockIn3));
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("StockIn/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, long id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var isExist = await _stockindetailService.IsAnyAsync(c => c.StockInId == id);
            if (isExist)
            {
                return new JsonResult((false, PubConst.StockIn3));
            }
            else
            {
                var flag1 = await _stockindetailService.UpdateAsync(
                     new WmsStockindetail { IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime },
                     c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate },
                     c => c.StockInId == id
                 );

                var flag2 = await _stockinService.UpdateAsync(
                    new WmsStockin { IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime },
                    c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate },
                    c => c.StockInId == id
                );

                return new JsonResult(new { Success = flag1 && flag2, Message = flag1 && flag2 ? PubConst.Delete1 : PubConst.Delete2 });
            }
        }

        [HttpGet]
        [OperationLog(LogType.delete)]
        public async Task<IActionResult> DeleteDetailAsync(string token, long userId, long id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var flag = await _stockindetailService.UpdateAsync(
                 new WmsStockindetail { IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime },
                 c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate },
                 c => c.StockInDetailId == id
                 );
            return new JsonResult((flag, flag ? PubConst.Delete1 : PubConst.Delete2));
        }
    }
}

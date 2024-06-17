using IServices;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Extensions;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IServices.Sys;
using IServices.Wms;
using DB.Models;
using NetTaste;

namespace  QRSFactoryWmsAPI.Controllers.Wms
{
    public class SupplierController : BaseController
    {
        private readonly IWms_SupplierService _supplierService;
        private readonly IWms_StockinService _stockinService;
        private readonly ISys_IdentityService _identityService;
        private readonly string NowUrl = "/Supplier";

        public SupplierController(
            IWms_StockinService stockinService,
            IWms_SupplierService supplierService,
            ISys_IdentityService identityService)
        {
            _stockinService = stockinService;
            _supplierService = supplierService;
            _identityService = identityService;
        }

        [HttpPost]
        [OperationLog(LogType.select)]
        public async Task<IActionResult> ListAsync(string token, long userId, Bootstrap.BootstrapParams bootstrap)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var sd =await _supplierService.PageListAsync(bootstrap);
            return new JsonResult(sd);
        }


        [HttpPost]
        [OperationLog(LogType.addOrUpdate)]
        public async Task<IActionResult> AddOrUpdateAsync(string token, long userId, WmsSupplier model, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            if (Id.IsZero())
            {
                if (await _supplierService.IsAnyAsync(c => c.SupplierNo == model.SupplierNo || c.SupplierName == model.SupplierName))
                {
                    return new JsonResult((false, PubConst.Supplier1));
                }
                model.SupplierId = PubId.SnowflakeId;
                model.CreateBy = UserDtoCache.UserId;
                bool flag =await _supplierService.InsertAsync(model);
                return new JsonResult(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
            }
            else
            {
                model.SupplierId = Id;
                model.ModifiedBy = UserDtoCache.UserId;
                model.ModifiedDate = DateTimeExt.DateTime;
                var flag = await _supplierService.UpdateAsync(model);
                return new JsonResult(flag ? (flag, PubConst.Update1) : (flag, PubConst.Update2));
            }
        }

        [HttpGet]
        [OperationLog(LogType.delete)]
        public async Task<IActionResult> DeleteAsync(long userId, long Id, string token)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var isDel = _stockinService.QueryableToSingleAsync(c => c.SupplierId == Id);
            if (!isDel.IsNullT())
            {
                return new JsonResult((false, PubConst.Supplier2));
            }
            var flag =await _supplierService.UpdateAsync(new WmsSupplier { SupplierId = Id, IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            return new JsonResult(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        }

        [HttpGet]
        public async Task<IActionResult> SearchAsync(string token, long userId, string text)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var bootstrap = new Bootstrap.BootstrapParams
            {
                limit = 100,
                offset = 0,
                sort = "CreateDate",
                search = text,
                order = "desc"
            };
            var json = await _supplierService.PageListAsync(bootstrap);
            return new JsonResult(json);
        }
    }
}
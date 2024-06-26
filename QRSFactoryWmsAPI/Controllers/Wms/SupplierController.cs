using AngleSharp.Dom;
using DB.Dto;
using DB.Models;
using IServices.Sys;
using IServices.Wms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Extensions;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;

namespace QRSFactoryWmsAPI.Controllers.Wms
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
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [Route("Supplier/List")]
        public async Task<IActionResult> ListAsync(string token, long userId, [FromForm] string bootstrap)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var sd = await _supplierService.PageListAsync(bootstrapObject);
            return Ok(sd);
        }

        [HttpPost]
        [OperationLog(LogType.select)]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [Route("Supplier/GetSupplierList")]
        public async Task<IActionResult> GetSupplierListAsync(string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var sd = await _supplierService.GetSupplierList();
            return Ok(sd);
        }

        [HttpPost]
        [OperationLog(LogType.addOrUpdate)]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [Route("Supplier/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync(string token, long userId, [FromForm] string model, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            if (Id==0)
            {
                var modelObject = JsonConvert.DeserializeObject<WmsSupplier>(model);
                if (await _supplierService.IsAnyAsync(c => c.SupplierNo == modelObject.SupplierNo || c.SupplierName == modelObject.SupplierName))
                {
                    return Ok((false, PubConst.Supplier1));
                }
                modelObject.SupplierId = PubId.SnowflakeId;
                modelObject.CreateBy = userId;
                modelObject.CreateDate = DateTimeExt.DateTime;
                modelObject.ModifiedBy = userId;
                modelObject.ModifiedDate = DateTimeExt.DateTime;
                modelObject.IsDel = 1;
                bool flag = await _supplierService.InsertAsync(modelObject);
                return Ok(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
            }
            else
            {
                var modelObject = JsonConvert.DeserializeObject<SupplierDto>(model);
                var entity = await _supplierService.QueryableToSingleAsync(s => s.SupplierId == Id&&s.IsDel==1);

                entity.SupplierNo = modelObject.SupplierNo;
                entity.SupplierName = modelObject.SupplierName;
                entity.Address = modelObject.Address;
                entity.Tel = modelObject.Tel;
                entity.Email = modelObject.Email;
                entity.SupplierNo = modelObject.SupplierNo;
                entity.SupplierPerson = modelObject.SupplierPerson;
                entity.SupplierLevel = modelObject.SupplierLevel;
                entity.Remark = modelObject.Remark;


                entity.ModifiedBy = userId;
                entity.ModifiedDate = DateTimeExt.DateTime;
                entity.IsDel = 1;
                var flag = await _supplierService.UpdateAsync(entity);
                return Ok(flag ? (flag, PubConst.Update1) : (flag, PubConst.Update2));
            }
        }

        [HttpPost]
        [OperationLog(LogType.delete)]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [Route("Supplier/Delete")]
        public async Task<IActionResult> DeleteAsync(long userId, long Id, string token)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var isDel = _stockinService.QueryableToSingleAsync(c => c.SupplierId == Id);
            if (!isDel.IsNullT())
            {
                return Ok((false, PubConst.Supplier2));
            }
            var flag = await _supplierService.UpdateAsync(new WmsSupplier { SupplierId = Id, IsDel = 0, ModifiedBy = userId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            return Ok(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        }
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("Supplier/Search")]
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
            var json = await _supplierService.PageListAsync(bootstrap);
            return Ok(json);
        }
    }
}
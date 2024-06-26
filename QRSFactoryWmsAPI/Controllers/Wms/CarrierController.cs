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
using Qiu.Utils.Extensions;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using System.CodeDom;

namespace QRSFactoryWmsAPI.Controllers.Wms
{
    public class CarrierController : BaseController
    {
        private readonly IWms_CarrierService _carrierService;
        private readonly ISys_IdentityService _identityService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly string NowUrl = "/Carrier";

        public CarrierController(IWms_CarrierService carrierService,
            ISys_IdentityService identityService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            IMediator mediator)
        {
            _carrierService = carrierService;
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
        [Route("Carrier/List")]
        public async Task<IActionResult> ListAsync(string token, long userId, [FromForm] string bootstrap)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var sd = await _carrierService.PageListAsync(bootstrapObject);
            return Ok(sd);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("Carrier/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync(string token, long userId, [FromForm] string model, string id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            if (id.IsEmptyZero())
            {
                var modelObject = JsonConvert.DeserializeObject<WmsCarrier>(model);
                if (await _carrierService.IsAnyAsync(c => c.CarrierNo == modelObject.CarrierNo || c.CarrierName == modelObject.CarrierName))
                {
                    return Ok((false, PubConst.Carrier1));
                }
                modelObject.CarrierId = PubId.SnowflakeId;
                modelObject.IsDel = 1;
                modelObject.CreateBy = userId;
                modelObject.CreateDate = DateTime.UtcNow;
                modelObject.ModifiedBy = userId;
                modelObject.ModifiedDate = DateTime.UtcNow;
                bool flag = await _carrierService.InsertAsync(modelObject);
                return Ok((flag, PubConst.Add1));
            }
            else
            {
                var modelObject = JsonConvert.DeserializeObject<CarrierDto>(model);
                var entity = await _carrierService.QueryableToSingleAsync(c => c.CarrierId == id.ToInt64()&&c.IsDel==1);
                entity.CarrierNo = modelObject.CarrierNo;
                entity.CarrierName = modelObject.CarrierName;
                entity.Address = modelObject.Address;
                entity.Tel = modelObject.Tel;
                entity.Email = modelObject.Email;
                entity.CarrierNo = modelObject.CarrierNo;
                entity.CarrierPerson = modelObject.CarrierPerson;
                entity.CarrierLevel = modelObject.CarrierLevel;
                entity.Remark = modelObject.Remark;

                entity.ModifiedBy = userId;
                entity.ModifiedDate = DateTime.UtcNow;
                var flag = await _carrierService.UpdateAsync(entity);
                return Ok((flag, PubConst.Update1));
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Carrier/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, long id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var flag = await _carrierService.UpdateAsync(new WmsCarrier { CarrierId = id, IsDel = 0, ModifiedBy = userId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            return Ok((flag, PubConst.Delete1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [Route("Carrier/Search")]
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
            var json = await _carrierService.PageListAsync(bootstrap);
            return Content(json);
        }
    }
}
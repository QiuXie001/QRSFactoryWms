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

namespace QRSFactoryWmsAPI.Controllers.Wms
{
    public class DeliveryController : BaseController
    {
        private readonly IWms_DeliveryService _deliveryServices;
        private readonly ISys_IdentityService _identityService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly string NowUrl = "/Delivery";

        public DeliveryController(
            IWms_DeliveryService deliveryServices,
            ISys_IdentityService identityService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            IMediator mediator)
        {
            _deliveryServices = deliveryServices;
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
        [Route("Delivery/List")]
        public async Task<IActionResult> ListAsync([FromForm] string bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var sd = await _deliveryServices.PageListAsync(bootstrapObject);
            return Ok(sd);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("Delivery/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync([FromForm] string model, string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var modelObject = JsonConvert.DeserializeObject<WmsDelivery>(model);
            if (model.IsEmpty())
            {

                if (!modelObject.TrackingNo.IsEmpty())
                {
                    if (await _deliveryServices.IsAnyAsync(c => c.TrackingNo == modelObject.TrackingNo))
                    {
                        return Ok((false, PubConst.Delivery3));
                    }
                }

                modelObject.DeliveryId = PubId.SnowflakeId;
                modelObject.CreateBy = userId;
                modelObject.CreateDate = DateTime.UtcNow;
                modelObject.ModifiedBy = userId;
                modelObject.ModifiedDate = DateTimeExt.DateTime;
                var flag = await _deliveryServices.DeliveryAsync(modelObject);
                return Ok((flag, PubConst.Add1));
            }
            else
            {
                modelObject.DeliveryId = Id.ToInt64();
                modelObject.ModifiedBy = userId;
                modelObject.ModifiedDate = DateTimeExt.DateTime;
                var flag = await _deliveryServices.UpdateAsync(modelObject);
                return Ok((flag, PubConst.Update1));
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Delivery/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var flag = await _deliveryServices.UpdateAsync(new WmsDelivery { DeliveryId = Id, IsDel = 1, ModifiedBy = userId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            return Ok((flag, PubConst.Delete1));
        }
    }
}

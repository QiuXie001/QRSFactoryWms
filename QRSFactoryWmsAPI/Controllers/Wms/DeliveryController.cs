﻿using IServices;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Extensions;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IServices.Wms;
using Microsoft.AspNetCore.Authorization;
using DB.Models;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Services;
using Newtonsoft.Json;

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
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var sd = await _deliveryServices.PageListAsync(bootstrapObject);
            return new JsonResult(sd);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("Delivery/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync(WmsDelivery model, string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            if (model.IsEmpty())
            {
                if (!model.TrackingNo.IsEmpty())
                {
                    if (await _deliveryServices.IsAnyAsync(c => c.TrackingNo == model.TrackingNo))
                    {
                        return new JsonResult((false, PubConst.Delivery3));
                    }
                }
                model.DeliveryId = PubId.SnowflakeId;
                model.CreateBy = UserDtoCache.UserId;
                model.CreateDate = DateTime.UtcNow;
                model.ModifiedBy = UserDtoCache.UserId;
                model.ModifiedDate = DateTimeExt.DateTime;
                var flag = await _deliveryServices.DeliveryAsync(model);
                return new JsonResult((flag, PubConst.Add1));
            }
            else
            {
                model.DeliveryId = Id.ToInt64();
                model.ModifiedBy = UserDtoCache.UserId;
                model.ModifiedDate = DateTimeExt.DateTime;
                var flag = await _deliveryServices.UpdateAsync(model);
                return new JsonResult((flag, PubConst.Update1));
            }
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Delivery/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var flag = await _deliveryServices.UpdateAsync(new WmsDelivery { DeliveryId = Id, IsDel = 1, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            return new JsonResult((flag, PubConst.Delete1));
        }
    }
}

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
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Extensions;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using SqlSugar;

namespace QRSFactoryWmsAPI.Controllers.Wms
{
    public class ReservoirareaController : BaseController
    {
        private readonly IWms_ReservoirareaService _reservoirareaService;
        private readonly IWms_StoragerackService _storagerackService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly ISys_IdentityService _identityService;
        private readonly string NowUrl = "/Reservoirarea";

        public ReservoirareaController(
            IWms_StoragerackService storagerackService,
            IWms_ReservoirareaService reservoirareaService,
            ISys_IdentityService identityService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            IMediator mediator)
        {
            _storagerackService = storagerackService;
            _reservoirareaService = reservoirareaService;
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
        [Route("Reservoirarea/List")]
        public async Task<IActionResult> ListAsync(string token, long userId, [FromForm] string bootstrap)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var sd = await _reservoirareaService.PageListAsync(bootstrapObject);
            return Ok(sd);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("Reservoirarea/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync(string token, long userId, [FromForm] string model, string id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            if (id.IsEmptyZero())
            {
                var modelObject = JsonConvert.DeserializeObject<WmsReservoirarea>(model);
                if (await _reservoirareaService.IsAnyAsync(c => c.ReservoirAreaNo == modelObject.ReservoirAreaNo || c.ReservoirAreaName == modelObject.ReservoirAreaName))
                {
                    return Ok((false, PubConst.Warehouse4));
                }
                modelObject.ReservoirAreaId = PubId.SnowflakeId;
                modelObject.ReservoirAreaNo = modelObject.ReservoirAreaNo;
                modelObject.ReservoirAreaName = modelObject.ReservoirAreaName;
                modelObject.WarehouseId = modelObject.WarehouseId;
                modelObject.Remark = modelObject.Remark;

                modelObject.IsDel = 1;
                modelObject.CreateBy = userId;
                modelObject.CreateDate = DateTime.UtcNow;
                modelObject.ModifiedBy = userId;
                modelObject.ModifiedDate = DateTime.UtcNow;
                bool flag = await _reservoirareaService.InsertAsync(modelObject);
                return Ok((flag, PubConst.Add1));
            }
            else
            {
                var modelObject = JsonConvert.DeserializeObject<ReservoirareaDto>(model);
                var entity = await _reservoirareaService.QueryableToSingleAsync(c => c.ReservoirAreaId == modelObject.ReservoirAreaId);
                entity.ReservoirAreaNo = modelObject.ReservoirAreaNo;
                entity.ReservoirAreaName = modelObject.ReservoirAreaName;
                entity.WarehouseId = modelObject.WarehouseId;
                entity.Remark = modelObject.Remark;

                entity.IsDel = 1;
                entity.ModifiedBy = userId;
                entity.ModifiedDate = DateTime.UtcNow;
                var flag = await _reservoirareaService.UpdateAsync(entity);
                return Ok((flag, PubConst.Update1));
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Reservoirarea/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, string id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var isExist = await _storagerackService.IsAnyAsync(c => c.ReservoirAreaId == SqlFunc.ToInt64(id));
            if (isExist)
            {
                return Ok((false, PubConst.Warehouse3));
            }
            else
            {
                var flag = await _reservoirareaService.UpdateAsync(new WmsReservoirarea { ReservoirAreaId = SqlFunc.ToInt64(id), IsDel = 0, ModifiedBy = userId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
                return Ok((flag, PubConst.Delete1));
            }
        }
    }
}

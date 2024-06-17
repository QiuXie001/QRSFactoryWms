using IServices;
using Microsoft.AspNetCore.Mvc;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Extensions;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IServices.Sys;
using IServices.Wms;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using DB.Models;
using SqlSugar;

namespace  QRSFactoryWmsAPI.Controllers.Wms
{
    public class WarehouseController : BaseController
    {
        private readonly IWms_WarehouseService _warehouseService;
        private readonly IWms_ReservoirareaService _reservoirareaService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly ISys_IdentityService _identityService;
        private readonly string NowUrl = "/Warehouse";

        public WarehouseController(IWms_WarehouseService warehouseService,
            IWms_ReservoirareaService reservoirareaService,
            ISys_IdentityService identityService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            IMediator mediator)
        {
            _warehouseService = warehouseService;
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
        [Route("Warehouse/List")]
        public async Task<IActionResult>ListAsync(string token, long userId, Bootstrap.BootstrapParams bootstrap)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var sd =await _warehouseService.PageListAsync(bootstrap);
            return new JsonResult(sd);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("Warehouse/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync(string token, long userId, WmsWarehouse model, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            if (Id.IsZero())
            {
                if (await _warehouseService.IsAnyAsync(c => c.WarehouseNo == model.WarehouseNo || c.WarehouseName == model.WarehouseName))
                {
                    return new JsonResult((false, PubConst.Warehouse1));
                }
                model.WarehouseId = PubId.SnowflakeId;
                model.CreateBy = UserDtoCache.UserId;
                bool flag =await _warehouseService.InsertAsync(model);
                return new JsonResult(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
            }
            else
            {
                model.WarehouseId = Id.ToInt64();
                model.ModifiedBy = UserDtoCache.UserId;
                model.ModifiedDate = DateTimeExt.DateTime;
                var flag =await _warehouseService.UpdateAsync(model);
                return new JsonResult(flag ? (flag, PubConst.Update1) : (flag, PubConst.Update2));
            }
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Warehouse/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var isExist =await _reservoirareaService.IsAnyAsync(c => c.WarehouseId == Id);
            if (isExist)
            {
                return new JsonResult((false, PubConst.Warehouse2));
            }
            else
            {
                var flag =await _warehouseService.UpdateAsync(new WmsWarehouse { WarehouseId = Id, IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
                return new JsonResult(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
            }
        }
    }
}
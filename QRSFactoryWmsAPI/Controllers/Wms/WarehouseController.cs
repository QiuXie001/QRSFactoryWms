﻿using DB.Dto;
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
        [Route("Warehouse/GetWarehouseList")]
        public async Task<IActionResult> GetWarehouseListAsync(string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var json = await _warehouseService.GetWarehouseList();
            return Ok(json);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("Warehouse/List")]
        public async Task<IActionResult> ListAsync(string token, long userId, [FromForm] string bootstrap)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;

            var sd = await _warehouseService.PageListAsync(bootstrapObject);
            return Ok(sd);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("Warehouse/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync(string token, long userId, [FromForm] string model, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            if (Id == 0)
            {
                var modelObject = JsonConvert.DeserializeObject<WmsWarehouse>(model);
                if (await _warehouseService.IsAnyAsync(c => c.WarehouseNo == modelObject.WarehouseNo || c.WarehouseName == modelObject.WarehouseName))
                {
                    return Ok((false, PubConst.Warehouse1));
                }
                modelObject.WarehouseId = PubId.SnowflakeId;
                modelObject.CreateBy = userId;
                modelObject.CreateDate = DateTime.UtcNow;
                modelObject.ModifiedBy = userId;
                modelObject.ModifiedDate = DateTime.UtcNow;
                modelObject.IsDel = 1;
                bool flag = await _warehouseService.InsertAsync(modelObject);
                return Ok(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
            }
            else
            {
                var modelObject = JsonConvert.DeserializeObject<WarehouseDto>(model);
                var entity = await _warehouseService.QueryableToSingleAsync(c => c.WarehouseId == Id);
                entity.WarehouseNo = modelObject.WarehouseNo;
                entity.WarehouseName = modelObject.WarehouseName;
                entity.Remark = modelObject.Remark;
                entity.ModifiedBy = userId;
                entity.ModifiedDate = DateTime.UtcNow;
                var flag = await _warehouseService.UpdateAsync(entity);
                return Ok(flag ? (flag, PubConst.Update1) : (flag, PubConst.Update2));
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Warehouse/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var isExist = await _reservoirareaService.IsAnyAsync(c => c.WarehouseId == Id);
            if (isExist)
            {
                return Ok((false, PubConst.Warehouse2));
            }
            else
            {
                var entity = await _warehouseService.QueryableToSingleAsync(w => w.WarehouseId == Id && w.IsDel == 1);
                entity.IsDel = 0;
                entity.ModifiedBy = userId;
                entity.ModifiedDate = DateTime.UtcNow;
                var flag = await _warehouseService.UpdateAsync(entity);
                return Ok(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
            }
        }
    }
}
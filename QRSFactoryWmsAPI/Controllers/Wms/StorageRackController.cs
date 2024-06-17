﻿using IServices;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IServices.Wms;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http;
using IServices.Sys;
using Services;
using Microsoft.AspNetCore.Cors;
using DB.Models;
using SqlSugar;

namespace QRSFactoryWmsAPI.Controllers.Wms
{
    public class StorageRackController : BaseController
    {
        private readonly IWms_WarehouseService _warehouseService;
        private readonly IWms_StoragerackService _storagerackService;
        private readonly IWms_ReservoirareaService _reservoirareaService;
        private readonly IWms_MaterialService _materialService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly ISys_IdentityService _identityService;
        private readonly string NowUrl = "/StorageRack";

        public StorageRackController(
            IWms_WarehouseService warehouseService,
            IWms_StoragerackService storagerackService,
            IWms_ReservoirareaService reservoirareaService,
            IWms_MaterialService materialService,
            IMediator mediator,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            ISys_IdentityService identityService)
        {
            _warehouseService = warehouseService;
            _storagerackService = storagerackService;
            _reservoirareaService = reservoirareaService;
            _materialService = materialService;
            _mediator = mediator;
            _httpContext = httpContext;
            _configuration = configuration;
            _identityService = identityService;
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("StorageRack/GetReservoirarea")]
        public async Task<IActionResult> GetReservoirareaAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var json = await _reservoirareaService.QueryableToSingleAsync(c => c.ReservoirAreaId == Id);
            return new JsonResult(json);
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("StorageRack/GetStoragerack")]
        public async Task<IActionResult> GetStoragerackAsync(long Id, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var json = await _storagerackService.QueryableToSingleAsync(c => c.StorageRackId == Id);
            return new JsonResult(json);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("StorageRack/List")]
        public async Task<IActionResult> ListAsync(string token, long userId, Bootstrap.BootstrapParams bootstrap)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            if (bootstrap._ == null)
                bootstrap = PubConst.DefaultBootstrapParams;
            var sd = await _storagerackService.PageListAsync(bootstrap);
            return new JsonResult(sd);
        }
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("StorageRack/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync(WmsStoragerack model, long Id, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            if (Id.IsZero())
            {
                if (await _storagerackService.IsAnyAsync(c => c.StorageRackNo == model.StorageRackNo || c.StorageRackName == model.StorageRackNo))
                {
                    return BootJsonH((false, PubConst.Warehouse5));
                }
                model.StorageRackId = PubId.SnowflakeId;
                model.CreateBy = UserDtoCache.UserId;
                bool flag = await _storagerackService.InsertAsync(model);
                return new JsonResult(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
            }
            else
            {
                model.StorageRackId = Id;
                model.ModifiedBy = UserDtoCache.UserId;
                model.ModifiedDate = DateTimeExt.DateTime;
                var flag =await _storagerackService.UpdateAsync(model);
                return new JsonResult(flag ? (flag, PubConst.Update1) : (flag, PubConst.Update2));
            }
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("StorageRack/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            //判断有没有物料
            var isExist =await _materialService.IsAnyAsync(c => c.StoragerackId ==Id);
            if (isExist)
            {
                return new JsonResult((false, PubConst.Warehouse6));
            }
            else
            {
                var flag =await _storagerackService.UpdateAsync(new WmsStoragerack { StorageRackId =Id, IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
                return new JsonResult(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
            }
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.search)]
        [Route("StorageRack/Search")]
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
            var json =await _storagerackService.PageListAsync(bootstrap);
            return new JsonResult(json);
        }
    }
}
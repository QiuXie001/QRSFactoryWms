using AngleSharp.Common;
using DB.Dto;
using DB.Models;
using IServices.Sys;
using IServices.Wms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Extensions;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
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

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("StorageRack/GetReservoirareaList")]
        public async Task<IActionResult> GetReservoirareaListAsync(string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var json = await _reservoirareaService.GetReservoirareaList();
            return Ok(json);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("StorageRack/GetStorageRackList")]
        public async Task<IActionResult> GetStorageRackListAsync(string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var json = await _storagerackService.GetStorageRackList();
            return Ok(json);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("StorageRack/GetReservoirarea")]
        public async Task<IActionResult> GetReservoirareaAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var json = await _reservoirareaService.QueryableToListAsync(c => c.WarehouseId == Id);
            var result = json.ToDictionary(
                item => item.ReservoirAreaId,
                item => item.ReservoirAreaName
            );
            return Ok(result);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("StorageRack/GetStoragerack")]
        public async Task<IActionResult> GetStoragerackAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var json = await _storagerackService.QueryableToListAsync(c => c.ReservoirAreaId == Id);
            var result = json.ToDictionary(
                item => item.StorageRackId,
                item => item.StorageRackName
            );
            return Ok(result);
        }



        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("StorageRack/List")]
        public async Task<IActionResult> ListAsync(string token, long userId, [FromForm] string bootstrap)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var sd = await _storagerackService.PageListAsync(bootstrapObject);
            return Ok(sd);
        }
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("StorageRack/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync([FromForm] string model, long Id, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            if (Id == 0)
            {
                var modelObject = JsonConvert.DeserializeObject<WmsStoragerack>(model);
                if (await _storagerackService.IsAnyAsync(c => c.StorageRackNo == modelObject.StorageRackNo || c.StorageRackName == modelObject.StorageRackNo))
                {
                    return BootJsonH((false, PubConst.Warehouse5));
                }
                modelObject.StorageRackId = PubId.SnowflakeId;
                modelObject.IsDel = 1;
                modelObject.CreateBy = userId;
                modelObject.CreateDate = DateTime.UtcNow;
                modelObject.ModifiedBy = userId;
                modelObject.ModifiedDate = DateTime.UtcNow;
                bool flag = await _storagerackService.InsertAsync(modelObject);
                return Ok(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
            }
            else
            {
                var modelObject = JsonConvert.DeserializeObject<StoragerackDto>(model);
                var entity = await _storagerackService.QueryableToSingleAsync(s => s.StorageRackId == modelObject.StorageRackId && s.IsDel==1);
                
                entity.StorageRackNo = modelObject.StorageRackNo;
                entity.StorageRackName = modelObject.StorageRackName;
                entity.WarehouseId = modelObject.WarehouseId;
                entity.ReservoirAreaId = modelObject.ReservoirAreaId;

                entity.CreateBy = userId;
                entity.CreateDate = DateTime.UtcNow;
                entity.ModifiedBy = userId;
                entity.ModifiedDate = DateTime.UtcNow;
                var flag = await _storagerackService.UpdateAsync(entity);
                return Ok(flag ? (flag, PubConst.Update1) : (flag, PubConst.Update2));
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("StorageRack/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, long Id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            //判断有没有物料
            var isExist = await _materialService.IsAnyAsync(c => c.StoragerackId == Id);
            if (isExist)
            {
                return Ok((false, PubConst.Warehouse6));
            }
            else
            {
                var flag = await _storagerackService.UpdateAsync(new WmsStoragerack { StorageRackId = Id, IsDel = 0, ModifiedBy = userId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
                return Ok(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.search)]
        [Route("StorageRack/Search")]
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
            var json = await _storagerackService.PageListAsync(bootstrap);
            return Ok(json);
        }
    }
}
using DB.Dto;
using DB.Models;
using IServices.Sys;
using IServices.Wms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Extensions;
using Qiu.Utils.Files;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Security;
using Qiu.Utils.Table;
using SqlSugar;

namespace QRSFactoryWmsAPI.Controllers.Wms
{
    public class MaterialController : BaseController
    {
        private readonly IWms_MaterialService _materialService;
        private readonly IWms_InventoryService _inventoryService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly ISys_IdentityService _identityService;
        private readonly ISys_DictService _dictService;
        private readonly string NowUrl = "/Material";

        public MaterialController(
            IWms_MaterialService materialService,
            IWms_InventoryService inventoryService,
            ISys_IdentityService identityService,
            ISys_DictService dictService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            IMediator mediator)
        {
            _materialService = materialService;
            _inventoryService = inventoryService;
            _mediator = mediator;
            _httpContext = httpContext;
            _configuration = configuration;
            _identityService = identityService;
            _dictService = dictService;
        }
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("Material/GetDictListByType")]
        public async Task<IActionResult> GetDictListByType(string token, long userId ,string type)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var item = await _dictService.GetDictListByType(type);
            return Ok(item);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("Material/GetMaterialList")]
        public async Task<IActionResult> GetMaterialList(string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var item = await _materialService.GetMaterialList();
            return Ok(item);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("Material/List")]
        public async Task<IActionResult> ListAsync(string token, long userId, [FromForm] string bootstrap)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var sd = await _materialService.PageListAsync(bootstrapObject);
            return Ok(sd);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("Material/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync(string token, long userId, [FromForm] string model, string id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            if (id.IsEmptyZero())
            {
                var modelObject = JsonConvert.DeserializeObject<WmsMaterial>(model);
                modelObject.IsDel = 1;
                if (await _materialService.IsAnyAsync(c => c.MaterialNo == modelObject.MaterialNo || c.MaterialName == modelObject.MaterialName))
                {
                    return Ok((false, PubConst.Material1));
                }
                modelObject.MaterialId = PubId.SnowflakeId;
                modelObject.CreateBy = userId;
                modelObject.CreateDate = DateTimeExt.DateTime;
                modelObject.ModifiedBy = userId;
                modelObject.ModifiedDate = DateTimeExt.DateTime;
                bool flag = await _materialService.InsertAsync(modelObject);
                return Ok((flag, PubConst.Add1));
            }
            else
            {
                var modelObject = JsonConvert.DeserializeObject<MaterialDto>(model);
                var entity = await _materialService.QueryableToSingleAsync(c => c.MaterialId == long.Parse(id));
                entity.MaterialNo = modelObject.MaterialNo;
                entity.MaterialName = modelObject.MaterialName;
                entity.MaterialTypeId = modelObject.MaterialTypeId;
                entity.UnitId = modelObject.UnitId;
                entity.WarehouseId = modelObject.WarehouseId;
                entity.StoragerackId = modelObject.StoragerackId;
                entity.ReservoirAreaId = modelObject.ReservoirAreaId;
                entity.ExpiryDate = modelObject.ExpiryDate;
                entity.Remark = modelObject.Remark;
                entity.ModifiedBy = userId;
                entity.ModifiedDate = DateTimeExt.DateTime;
                var flag = await _materialService.UpdateAsync(entity);
                return Ok((flag, PubConst.Update1));
            }
        }
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Material/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, string id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            // 判断库存数量，库存数量小于等于0，才能删除
            var isExist = await _inventoryService.IsAnyAsync(c => c.MaterialId == SqlFunc.ToInt64(id));
            if (isExist)
            {
                return Ok((false, PubConst.Material2));
            }
            else
            {
                var flag = await _materialService.UpdateAsync(new WmsMaterial { MaterialId = SqlFunc.ToInt64(id), IsDel = 0, ModifiedBy = userId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
                return Ok((flag, PubConst.Delete1));
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("Material/Search")]
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
            var json = await _materialService.PageListAsync(bootstrap);
            return Content(json);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.export)]
        [Route("Material/Export")]
        public async Task<IActionResult> ExportAsync(string token, long userId, [FromForm] string bootstrap)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return File(JsonL((false, PubConst.File8)).ToBytes(), ContentType.ContentTypeJson);
            }
            var bootstrapDto = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            var buffer = await _materialService.ExportListAsync(bootstrapDto);
            if (buffer.IsNull())
            {
                return File(JsonL((false, PubConst.File8)).ToBytes(), ContentType.ContentTypeJson);
            }
            return File(buffer, ContentType.ContentTypeFile, DateTimeExt.GetDateTimeS(DateTimeExt.DateTimeFormatString) + "-" + EncoderUtil.UrlHttpUtilityEncoder("物料") + ".xlsx");
        }
    }
}
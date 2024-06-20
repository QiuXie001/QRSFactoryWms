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
using Qiu.Utils.Files;
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
        private readonly string NowUrl = "/Material";

        public MaterialController(
            IWms_MaterialService materialService,
            IWms_InventoryService inventoryService,
            ISys_IdentityService identityService,
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
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [CheckMenu]
        public IActionResult Index()
        {
            return View();
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
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var sd = await _materialService.PageListAsync(bootstrapObject);
            return new JsonResult(sd);
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
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var modelObject = JsonConvert.DeserializeObject<WmsMaterial>(model);
            if (id.IsEmptyZero())
            {
                if (await _materialService.IsAnyAsync(c => c.MaterialNo == modelObject.MaterialNo || c.MaterialName == modelObject.MaterialName))
                {
                    return new JsonResult((false, PubConst.Material1));
                }
                modelObject.MaterialId = PubId.SnowflakeId;
                modelObject.CreateBy = UserDtoCache.UserId;
                modelObject.CreateDate = DateTimeExt.DateTime;
                modelObject.ModifiedBy = UserDtoCache.UserId;
                modelObject.ModifiedDate = DateTimeExt.DateTime;
                bool flag = await _materialService.InsertAsync(modelObject);
                return new JsonResult((flag, PubConst.Add1));
            }
            else
            {
                modelObject.MaterialId = id.ToInt64();
                modelObject.ModifiedBy = UserDtoCache.UserId;
                modelObject.ModifiedDate = DateTimeExt.DateTime;
                var flag = await _materialService.UpdateAsync(modelObject);
                return new JsonResult((flag, PubConst.Update1));
            }
        }
        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Material/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, string id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            // 判断库存数量，库存数量小于等于0，才能删除
            var isExist = await _inventoryService.IsAnyAsync(c => c.MaterialId == SqlFunc.ToInt64(id));
            if (isExist)
            {
                return new JsonResult((false, PubConst.Material2));
            }
            else
            {
                var flag = await _materialService.UpdateAsync(new WmsMaterial { MaterialId = SqlFunc.ToInt64(id), IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
                return new JsonResult((flag, PubConst.Delete1));
            }
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("Material/Search")]
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
            var json = await _materialService.PageListAsync(bootstrap);
            return Content(json);
        }

        [HttpGet]
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
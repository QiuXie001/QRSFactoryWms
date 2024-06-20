using IServices;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Extensions;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IServices.Wms;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http;
using DB.Models;
using IServices.Sys;
using Microsoft.AspNetCore.Cors;
using Services;
using SqlSugar;
using Newtonsoft.Json;

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
        public async Task<IActionResult> ListAsync(string token, long userId,[FromForm] string bootstrap)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var sd = await _reservoirareaService.PageListAsync(bootstrapObject);
            return new JsonResult(sd);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("Reservoirarea/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync(string token, long userId,[FromForm] string model, string id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var modelObject = JsonConvert.DeserializeObject<WmsReservoirarea>(model);
            if (id.IsEmptyZero())
            {
                if (await _reservoirareaService.IsAnyAsync( c => c.ReservoirAreaNo == modelObject.ReservoirAreaNo || _reservoirareaService.IsAnyAsync(c => c.ReservoirAreaNo == modelObject.ReservoirAreaNo || c.ReservoirAreaName == modelObject.ReservoirAreaName).Result))
                {
                    return new JsonResult((false, PubConst.Warehouse4));
                }
                modelObject.ReservoirAreaId = PubId.SnowflakeId;
                modelObject.CreateBy = UserDtoCache.UserId;
                bool flag = await _reservoirareaService.InsertAsync(modelObject);
                return new JsonResult((flag, PubConst.Add1));
            }
            else
            {
                modelObject.ReservoirAreaId = id.ToInt64();
                modelObject.ModifiedBy = UserDtoCache.UserId;
                modelObject.ModifiedDate = DateTimeExt.DateTime;
                var flag = await _reservoirareaService.UpdateAsync(modelObject);
                return new JsonResult((flag, PubConst.Update1));
            }
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Reservoirarea/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, string id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var isExist = await _storagerackService.IsAnyAsync(c => c.ReservoirAreaId == SqlFunc.ToInt64(id));
            if (isExist)
            {
                return new JsonResult((false, PubConst.Warehouse3));
            }
            else
            {
                var flag = await _reservoirareaService.UpdateAsync(new WmsReservoirarea { ReservoirAreaId = SqlFunc.ToInt64(id), IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
                return new JsonResult((flag, PubConst.Delete1));
            }
        }
    }
}

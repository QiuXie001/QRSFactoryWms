using DB.Models;
using IServices;
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
using System.Linq;
using System.Threading.Tasks;

namespace  QRSFactoryWmsAPI.Controllers.Wms
{
    public class CarrierController : BaseController
    {
        private readonly IWms_CarrierService _carrierService;
        private readonly ISys_IdentityService _identityService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly string NowUrl = "/Carrier";

        public CarrierController(IWms_CarrierService carrierService,
            ISys_IdentityService identityService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            IMediator mediator)
        {
            _carrierService = carrierService;
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
        [Route("Carrier/List")]
        public async Task<IActionResult> ListAsync(string token, long userId,[FromForm] string bootstrap )
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var sd = await _carrierService.PageListAsync(bootstrapObject);
            return new JsonResult(sd);
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("Carrier/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync(string token, long userId, [FromForm] string model, string id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var carrierObject = JsonConvert.DeserializeObject<WmsCarrier>(model);
            if (id.IsEmptyZero())
            {
               
                if (await _carrierService.IsAnyAsync(c => c.CarrierNo == carrierObject.CarrierNo || c.CarrierName == carrierObject.CarrierName))
                {
                    return new JsonResult((false, PubConst.Carrier1));
                }
                carrierObject.CarrierId = PubId.SnowflakeId;
                carrierObject.CreateBy = UserDtoCache.UserId;
                bool flag = await _carrierService.InsertAsync(carrierObject);
                return new JsonResult((flag, PubConst.Add1));
            }
            else
            {
                carrierObject.CarrierId = id.ToInt64();
                carrierObject.ModifiedBy = UserDtoCache.UserId;
                carrierObject.ModifiedDate = DateTimeExt.DateTime;
                var flag = await _carrierService.UpdateAsync(carrierObject);
                return new JsonResult((flag, PubConst.Update1));
            }
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Carrier/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, long id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var flag = await _carrierService.UpdateAsync(new WmsCarrier { CarrierId = id, IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate });
            return new JsonResult((flag, PubConst.Delete1));
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [Route("Carrier/Search")]
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
            var json = await _carrierService.PageListAsync(bootstrap);
            return Content(json);
        }
    }
}
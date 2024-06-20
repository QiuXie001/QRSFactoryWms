using IServices.Sys;
using IServices.Wms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qiu.Core.Dto;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Pub;

namespace QRSFactoryWmsAPI.Controllers.Wms
{
    public class InventoryRecordController : BaseController
    {
        private readonly IWms_InventoryrecordService _inventoryrecordService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly ISys_IdentityService _identityService;
        private readonly string NowUrl = "/InventoryRecord";

        public InventoryRecordController(
            IWms_InventoryrecordService inventoryrecordService,
            IMediator mediator,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            ISys_IdentityService identityService
            )
        {
            _inventoryrecordService = inventoryrecordService;
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
        [Route("InventoryRecord/List")]
        public async Task<IActionResult> ListAsync(string token, long userId, [FromForm] string bootstrap)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var bootstrapDto = JsonConvert.DeserializeObject<PubParams.InventoryBootstrapParams>(bootstrap);

            var sd = await _inventoryrecordService.PageListAsync(bootstrapDto);
            return new JsonResult(sd);
        }
    }
}

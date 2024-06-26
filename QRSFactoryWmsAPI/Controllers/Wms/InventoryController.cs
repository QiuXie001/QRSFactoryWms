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
    public class InventoryController : BaseController
    {
        private readonly IWms_InventoryService _inventoryService;
        private readonly IWms_StoragerackService _storagerackService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly ISys_IdentityService _identityService;
        private readonly string NowUrl = "/Inventory";

        public InventoryController(
            IWms_StoragerackService storagerackService,
            IWms_InventoryService inventoryService,
            ISys_IdentityService identityService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            IMediator mediator)
        {
            _storagerackService = storagerackService;
            _inventoryService = inventoryService;
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
        [Route("Inventory/List")]
        public async Task<IActionResult> ListAsync(string token, long userId, [FromForm] string bootstrap)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var bootstrapDto = JsonConvert.DeserializeObject<PubParams.InventoryBootstrapParams>(bootstrap);

            var sd = await _inventoryService.PageListAsync(bootstrapDto);
            return Ok(sd);
        }
    }
}

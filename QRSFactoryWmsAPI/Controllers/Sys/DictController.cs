using DB.Dto;
using DB.Models;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qiu.NetCore.Attributes;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Security;
using Qiu.Utils.Table;

namespace QRSFactoryWmsAPI.Controllers.Sys
{
    public class DictController : BaseController
    {
        private readonly ISys_DictService _dictService;
        private readonly ISys_IdentityService _identityService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;
        private readonly string NowUrl = "/Dict";
        public DictController(Xss xss,
            ISys_DictService dictService,
            ISys_IdentityService identityService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            IMediator mediator)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _dictService = dictService;
            _identityService = identityService;
            _xss = xss;
            _mediator = mediator;
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("Dict/GetDictListByType")]
        public async Task<IActionResult> GetDictListByType(string token, long userId, string type)
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
        [Route("Dict/GetPageList")]
        public async Task<string> GetPageList([FromForm] string bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return (false, PubConst.ValidateToken2).ToJson();
            }
            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var item = await _dictService.PageListAsync(bootstrapObject);
            return item;
        }
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.add)]
        [Route("Dict/Insert")]
        public async Task<IActionResult> Insert([FromForm] string dict, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var dictObject = JsonConvert.DeserializeObject<SysDict>(dict);
            dictObject.DictType = dictObject.DictType;
            dictObject.DictName = dictObject.DictName;
            dictObject.Remark = dictObject.Remark;
            dictObject.DictId = PubId.SnowflakeId;
            dictObject.IsDel = 1;
            dictObject.CreateBy = userId;
            dictObject.CreateDate = DateTime.UtcNow;
            dictObject.ModifiedBy = userId;
            dictObject.ModifiedDate = DateTime.UtcNow;
            var item = await _dictService.InsertAsync(dictObject);
            return Ok((item, PubConst.Add1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("Dict/Update")]
        public async Task<IActionResult> Update([FromForm] string dict, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var dictObject = JsonConvert.DeserializeObject<DictDto>(dict);
            var entity = await _dictService.QueryableToSingleAsync(d => d.DictId == dictObject.DictId&&d.IsDel==1);
            entity.DictType = dictObject.DictType;
            entity.DictName = dictObject.DictName;
            entity.Remark = dictObject.Remark;
            entity.IsDel = 1;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.UtcNow;
            var item = await _dictService.UpdateAsync(entity);
            return Ok((item, PubConst.Update1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Dict/Delete")]
        public async Task<IActionResult> Delete([FromForm] string dict, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var dictObject = JsonConvert.DeserializeObject<SysDict>(dict);
            var item = await _dictService.DeleteAsync(dictObject);
            return Ok((item, PubConst.Delete1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.disable)]
        [Route("Dict/Disable")]
        public async Task<IActionResult> Disable([FromForm] string dict, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var dictObject = JsonConvert.DeserializeObject<SysDict>(dict);
            dictObject.IsDel = 0;
            dictObject.ModifiedBy = userId;
            dictObject.ModifiedDate = DateTime.Now;
            var item = await _dictService.UpdateAsync(dictObject);
            return Ok((item, PubConst.Update2));
        }
    }
}

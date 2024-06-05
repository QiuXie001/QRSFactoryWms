using DB.Models;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Qiu.NetCore.NetCoreApp;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Security;
using Qiu.Utils.Table;

namespace QRSFactoryWmsAPI.Controllers.Sys
{
    public class LogController : BaseController
    {
        private readonly ISys_LogService _logService;
        private readonly ISys_IdentityService _identityService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;
        private readonly string NowUrl = "/Log";
        public LogController(Xss xss,
            ISys_LogService logService,
            ISys_IdentityService identityService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            IMediator mediator)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _logService = logService;
            _identityService = identityService;
            _xss = xss;
            _mediator = mediator;
        }
        [HttpGet]
        [EnableCors]
        [AllowAnonymous]
        [Route("Log/GetPageList")]
        public async Task<string> GetPageList(Bootstrap.BootstrapParams bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return (false, PubConst.ValidateToken2).ToJson();
            }
            if (bootstrap._ == null)
                bootstrap = PubConst.DefaultBootstrapParams;
            var item = await _logService.PageListAsync(bootstrap);
            await _logService.InsertAsync(new SysLog
            {
                LogId = PubId.SnowflakeId,
                Browser = GetBrowser(),
                CreateBy = userId,
                CreateDate = DateTime.UtcNow,
                Description = userId + "用户获取日志分页列表",
                LogIp = GetIp(),
                Url = GetUrl(),
                LogType = LogType.getList.ToString()
            });
            return item;
        }
    }
}

﻿using DB.Dto;
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
    public class DeptController : BaseController
    {
        private readonly ISys_DeptService _deptService;
        private readonly ISys_IdentityService _identityService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _configuration;
        private readonly Xss _xss;
        private readonly IMediator _mediator;
        private readonly string NowUrl = "/Dept";
        public DeptController(Xss xss,
            ISys_DeptService deptService,
            ISys_IdentityService identityService,
            IHttpContextAccessor httpContext,
            IConfiguration configuration,
            IMediator mediator)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            _deptService = deptService;
            _identityService = identityService;
            _xss = xss;
            _mediator = mediator;
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("Dept/GetPageList")]
        public async Task<string> GetPageList([FromForm] string bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return (false, PubConst.ValidateToken2).ToJson();
            }

            var bootstrapObject = JsonConvert.DeserializeObject<Bootstrap.BootstrapParams>(bootstrap);
            if (bootstrapObject == null || bootstrapObject._ == null)
                bootstrapObject = PubConst.DefaultBootstrapParams;
            var item = await _deptService.PageListAsync(bootstrapObject);
            return item;
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.add)]
        [Route("Dept/Insert")]
        public async Task<IActionResult> Insert([FromForm] string dept, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var deptObject = JsonConvert.DeserializeObject<SysDept>(dept);
            deptObject.DeptId = PubId.SnowflakeId;
            deptObject.CreateBy = userId;
            deptObject.CreateDate = DateTime.UtcNow;
            deptObject.ModifiedBy = userId;
            deptObject.ModifiedDate = DateTime.UtcNow;
            deptObject.IsDel = 1;
            var item = await _deptService.InsertAsync(deptObject);
            return Ok((item, PubConst.Add1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("Dept/Update")]
        public async Task<IActionResult> Update([FromForm] string dept, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var deptObject = JsonConvert.DeserializeObject<DeptDto>(dept);
            var entity = await _deptService.QueryableToSingleAsync(d => d.DeptId == deptObject.DeptId&&d.IsDel==1);
            entity.DeptNo = deptObject.DeptNo;
            entity.DeptName = deptObject.DeptName;
            entity.Remark = deptObject.Remark;
            entity.ModifiedBy = userId;
            entity.ModifiedDate = DateTime.UtcNow;
            var item = await _deptService.UpdateAsync(entity);
            return Ok((item, PubConst.Update1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("Dept/Delete")]
        public async Task<IActionResult> Delete([FromForm] string dept, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var deptObject = JsonConvert.DeserializeObject<SysDept>(dept);
            var item = await _deptService.DeleteAsync(deptObject);
            return Ok((item, PubConst.Delete1));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.disable)]
        [Route("Dept/Disable")]
        public async Task<IActionResult> Disable([FromForm] string dept, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }

            var deptObject = JsonConvert.DeserializeObject<SysDept>(dept);
            deptObject.IsDel = 0;
            deptObject.ModifiedBy = userId;
            deptObject.ModifiedDate = DateTime.Now;
            var item = await _deptService.UpdateAsync(deptObject);
            return Ok((item, PubConst.Update2));
        }
        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [Route("Dept/GetDeptList")]
        public async Task<IActionResult> GetDeptList(string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return Ok((false, PubConst.ValidateToken2));
            }
            var item = await _deptService.GetDeptList();
            return Ok(item);
        }
    }
}

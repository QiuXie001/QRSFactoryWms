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
using DB.Models;
using IServices.Sys;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Services;
using Qiu.Core.Dto;
using SqlSugar;
using DB.Dto;
using NetTaste;
using Qiu.Utils.Json;

namespace QRSFactoryWmsAPI.Controllers.Wms
{
    public class InventoryMoveController : BaseController
    {
        private readonly IWms_InventorymoveService _inventorymoveService;
        private readonly IWms_InvmovedetailService _invmovedetailService;
        private readonly ISys_SerialnumService _serialnumService;
        private readonly IWms_MaterialService _materialService;
        private readonly IWms_InventoryService _inventoryService;
        private readonly ISys_IdentityService _identityService;
        private readonly string NowUrl = "/InventoryMove";

        public InventoryMoveController(
            IWms_InventorymoveService inventorymoveService,
            IWms_InvmovedetailService invmovedetailService,
            ISys_SerialnumService serialnumService,
            IWms_MaterialService materialService,
            IWms_InventoryService inventoryService,
            ISys_IdentityService identityService
            )
        {
            _inventorymoveService = inventorymoveService;
            _invmovedetailService = invmovedetailService;
            _serialnumService = serialnumService;
            _materialService = materialService;
            _inventoryService = inventoryService;
            _identityService = identityService;
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("InventoryMove/List")]
        public async Task<IActionResult> ListAsync(PubParams.StatusBootstrapParams bootstrap, string token, long userId)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var sd = await _inventorymoveService.PageListAsync(bootstrap);
            return Content(sd);
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("InventoryMove/List")]
        public async Task<IActionResult> ListDetailAsync(string token, long userId,string pid)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var sd = await _invmovedetailService.PageListAsync(pid);
            return new JsonResult(sd);
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.select)]
        [Route("InventoryMove/Detail")]
        public async Task<IActionResult> DetailAsync(string token, long userId, long id, string pid )
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var model = new Inventorymove();
            var head = await _inventorymoveService.QueryableToSingleAsync(m => m.InventorymoveId == SqlFunc.ToInt64(pid) && m.IsDel == 1);
            model.AimStoragerackId = head.AimStoragerackId;
            model.SourceStoragerackId = head.SourceStoragerackId;
            if (id.IsZero())
            {
                model.InventorymoveId = long.Parse(pid);
                return new JsonResult(model);
            }
            else
            {
                var detail = await _invmovedetailService.QueryableToListAsync(c => c.InventorymoveId == SqlFunc.ToInt64(pid) && c.IsDel == 1);
                model.InventorymoveId = long.Parse(pid);
                model.Invmovedetails = detail.Select(c => new Invmovedetail
                {
                    MaterialId = c.MaterialId,
                    MaterialNo = _materialService.QueryableToSingleAsync(m => m.MaterialId == c.MaterialId).Result.MaterialNo,
                    MaterialName = _materialService.QueryableToSingleAsync(m => m.MaterialId == c.MaterialId).Result.MaterialName,
                    ActQty = c.ActQty,
                    Qty = _inventoryService.QueryableToSingleAsync(m => m.MaterialId == c.MaterialId && m.StoragerackId == model.SourceStoragerackId).Result.Qty,
                    AuditinId = c.AuditinId,
                    AuditinTime = c.AuditinTime,
                    CreateBy = c.CreateBy,
                    CreateDate = c.CreateDate,
                    InventorymoveId = c.InventorymoveId,
                    IsDel = c.IsDel,
                    ModifiedBy = c.ModifiedBy,
                    MoveDetailId = c.MoveDetailId,
                    ModifiedDate = c.ModifiedDate,
                    PlanQty = c.PlanQty,
                    Remark = c.Remark,
                    Status = c.Status,
                }).ToList();
                return new JsonResult(model);
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("InventoryMove/AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdateAsync(string token, long userId,WmsInventorymove model, long id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            if (id.IsZero())
            {
                model.InventorymoveNo = await _serialnumService.GetSerialnumAsync(UserDtoCache.UserId, "WmsInventorymove");
                model.InventorymoveId = PubId.SnowflakeId;
                model.Status = StockInStatus.initial.ToByte();
                model.CreateBy = UserDtoCache.UserId;
                model.CreateDate = DateTimeExt.DateTime;
                var flag = await _inventorymoveService.InsertAsync(model);
                return new JsonResult(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
            }
            else
            {
                model.InventorymoveId = id.ToInt64();
                model.ModifiedBy = UserDtoCache.UserId;
                model.ModifiedDate = DateTimeExt.DateTime;
                var flag = await _inventorymoveService.UpdateAsync(model);
                return new JsonResult(flag ? (flag, PubConst.Update1) : (flag, PubConst.Update2));
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.addOrUpdate)]
        [Route("InventoryMove/AddOrUpdateDetail")]
        public async Task<IActionResult> AddOrUpdateDetailAsync(string token, long userId, List<WmsInvmovedetail> list, long id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var exist = _invmovedetailService.QueryableToListAsync(c => c.InventorymoveId == SqlFunc.ToInt64(id));
            var modelList = new List<WmsInvmovedetail>();
            if (exist.IsNullT())
            {
                list.ForEach((c) =>
                {
                    c.Remark = c.Remark;
                    c.MoveDetailId = PubId.SnowflakeId;
                    c.Status = StockInStatus.initial.ToByte();
                    c.IsDel = 1;
                    c.CreateBy = UserDtoCache.UserId;
                    c.CreateDate = DateTimeExt.DateTime;
                    modelList.Add(c);
                });
                bool flag = await _invmovedetailService.InsertBatchAsync(modelList);
                return new JsonResult(flag ? (flag, PubConst.Add1) : (flag, PubConst.Add2));
            }
            else
            {
                await _invmovedetailService.UpdateAsync(new WmsInvmovedetail { IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate }, c => c.InventorymoveId == SqlFunc.ToInt64(id) && c.IsDel == 1);
                list.ForEach((c) =>
                {
                    c.Remark = c.Remark;
                    c.Status = StockInStatus.initial.ToByte();
                    c.MoveDetailId = PubId.SnowflakeId;
                    c.IsDel = 1;
                    c.CreateBy = UserDtoCache.UserId;
                    c.CreateDate = DateTimeExt.DateTime;
                    modelList.Add(c);
                });

                var flag = await _invmovedetailService.InsertBatchAsync(modelList);
                return new JsonResult(flag ? (flag, PubConst.Update1) : (flag, PubConst.Update2));
            }
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.update)]
        [Route("InventoryMove/Auditin")]
        public async Task<IActionResult> AuditinAsync(string token, long userId, long id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var list = _invmovedetailService.QueryableToListAsync(c => c.IsDel == 1 && c.InventorymoveId == SqlFunc.ToInt64(id));
            if (!list.IsEmpty())
            {
                return new JsonResult((false, PubConst.StockIn4));
            }
            var flag = await _inventorymoveService.AuditinAsync(UserDtoCache.UserId, id);
            return new JsonResult(flag ? (flag, PubConst.StockIn2) : (flag, PubConst.StockIn3));
        }

        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("InventoryMove/Delete")]
        public async Task<IActionResult> DeleteAsync(string token, long userId, long id )
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid ID.");
                }

                var flag1 = await _invmovedetailService.UpdateAsync(
                 new WmsInvmovedetail { IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime },
                 c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate },
                 c => c.MoveDetailId == id
                 );

                var flag2 = await _inventorymoveService.UpdateAsync(
                 new WmsInventorymove { IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime },
                 c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate },
                 c => c.InventorymoveId == id
                 );

                return new JsonResult(new { Success = flag1 && flag2, Message = flag1 && flag2 ? PubConst.Delete1 : PubConst.Delete2 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.delete)]
        [Route("InventoryMove/DeleteDetail")]
        public async Task<IActionResult> DeleteDetailAsync(string token, long userId, long id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var flag = await _invmovedetailService.UpdateAsync(
                 new WmsInvmovedetail { IsDel = 0, ModifiedBy = UserDtoCache.UserId, ModifiedDate = DateTimeExt.DateTime },
                 c => new { c.IsDel, c.ModifiedBy, c.ModifiedDate },
                 c => c.MoveDetailId == id
                 );
            return new JsonResult(flag ? (flag, PubConst.Delete1) : (flag, PubConst.Delete2));
        }

        [HttpGet]
        [EnableCors("CorsPolicy")]
        [Authorize]
        [AllowAnonymous]
        [OperationLog(LogType.getList)]
        [Route("InventoryMove/PreviewJson")]
        public async Task<IActionResult> PreviewJsonAsync(string token, long userId, long id)
        {
            if (!await _identityService.ValidateToken(token, userId, NowUrl))
            {
                return new JsonResult(false, PubConst.ValidateToken2);
            }
            var str = _inventorymoveService.PrintList(id);
            return new JsonResult(str);
        }
    }
}




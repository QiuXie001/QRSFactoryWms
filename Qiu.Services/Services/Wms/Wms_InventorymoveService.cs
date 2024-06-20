using DB.Dto;
using DB.Models;
using IRepository.Wms;
using IServices.Wms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Qiu.NetCore.DI;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Log;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using System.Text.Json;

namespace Services
{
    public class Wms_InventorymoveService : BaseService<WmsInventorymove>, IWms_InventorymoveService
    {
        private readonly IWms_InventorymoveRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public Wms_InventorymoveService(IWms_InventorymoveRepository repository,
            QrsfactoryWmsContext dbContext,
            IWebHostEnvironment env
            ) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
            _env = env;
        }

        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            int totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<WmsInventorymove>()
                .Include(w => w.SourceStoragerack)
                .Include(w => w.AimStoragerack)
                .Include(w => w.CreateByUser)
                .Include(w => w.ModifiedByUser)
                .Where(w => w.IsDel == 1)
                .Select(w => new
                {
                    InventorymoveId = w.InventorymoveId.ToString(),
                    w.InventorymoveNo,
                    w.Status,
                    SourceStorageRackNo = w.SourceStoragerack.StorageRackNo,
                    SourceStorageRackName = w.SourceStoragerack.StorageRackName,
                    AimStorageRackNo = w.AimStoragerack.StorageRackNo,
                    AimStorageRackName = w.AimStoragerack.StorageRackName,
                    w.IsDel,
                    w.Remark,
                    CName = w.CreateByUser.UserNickname,
                    w.CreateDate,
                    UName = w.ModifiedByUser.UserNickname,
                    w.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(w => w.InventorymoveNo.Contains(bootstrap.search));
            }

            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query = query.Where(w => w.CreateDate > bootstrap.datemin.ToDateTimeB() && w.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }

            if (bootstrap.order != null && bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(w => EF.Property<object>(w, bootstrap.sort));
            }
            else
            {
                query = query.OrderBy(w => EF.Property<object>(w, bootstrap.sort));
            }

            var list = await query.Skip((pageNumber - 1) * bootstrap.limit)
                                  .Take(bootstrap.limit)
                                  .ToListAsync();

            totalNumber = await query.CountAsync();

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }


        public async Task<bool> AuditinAsync(long userId, long InventorymoveId)
        {
            // 使用 DbContext 开始事务
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    // 查询 inventorymove 记录
                    var invmove = await _dbContext.Set<WmsInventorymove>().FindAsync(InventorymoveId);
                    if (invmove == null)
                    {
                        throw new Exception("Inventorymove not found.");
                    }

                    // 查询 inventorymove detail 记录
                    var invmovedetailList = await _dbContext.Set<WmsInvmovedetail>().Where(c => c.InventorymoveId == InventorymoveId).ToListAsync();

                    foreach (var invmovedetail in invmovedetailList)
                    {
                        // 查询目标库存记录
                        var targetInventory = await _dbContext.Set<WmsInventory>().Where(i => i.MaterialId == invmovedetail.MaterialId && i.StoragerackId == invmove.AimStoragerackId).FirstOrDefaultAsync();
                        if (targetInventory == null)
                        {
                            // 如果没有目标库存记录，则插入新记录
                            targetInventory = new WmsInventory
                            {
                                StoragerackId = invmove.AimStoragerackId,
                                CreateBy = userId,
                                InventoryId = PubId.SnowflakeId,
                                MaterialId = invmovedetail.MaterialId,
                                Qty = invmovedetail.ActQty
                            };
                            await _dbContext.Set<WmsInventory>().AddAsync(targetInventory);
                        }
                        else
                        {
                            // 如果有目标库存记录，则更新其数量
                            targetInventory.Qty += invmovedetail.ActQty;
                        }

                        // 更新库存记录
                        await _dbContext.SaveChangesAsync();

                        // 更新 inventorymove detail 记录状态
                        invmovedetail.Status = (byte)StockInStatus.egis;
                        invmovedetail.AuditinId = userId;
                        invmovedetail.AuditinTime = DateTimeExt.DateTime;
                        invmovedetail.ModifiedBy = userId;
                        invmovedetail.ModifiedDate = DateTimeExt.DateTime;
                        await _dbContext.SaveChangesAsync();
                    }

                    // 更新 inventorymove 主表状态
                    invmove.Status = (byte)StockInStatus.egis;
                    invmove.ModifiedBy = userId;
                    invmove.ModifiedDate = DateTimeExt.DateTime;
                    await _dbContext.SaveChangesAsync();

                    // 提交事务
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    // 回滚事务
                    transaction.Rollback();
                    var _nlog = ServiceResolve.Resolve<ILogUtil>();
                    _nlog.Error("审核失败" + ex);
                    return false;
                }
            }
        }



        public string PrintList(long InventorymoveId)
        {
            var flag1 = true;
            var flag2 = true;
            var list2 = new List<PrintListItem.Inventorymove>();

            try
            {
                // 查询 inventorymove 记录
                var invmove = _dbContext.Set<WmsInventorymove>().Where(s => s.IsDel == 1 && s.InventorymoveId == InventorymoveId).FirstOrDefault();
                if (invmove == null)
                {
                    flag1 = false;
                }
                else
                {
                    // 查询 inventorymove detail 记录
                    var invmovedetailList = _dbContext.Set<WmsInvmovedetail>().Where(s => s.IsDel == 1 && s.InventorymoveId == InventorymoveId).ToList();
                    foreach (var invmovedetail in invmovedetailList)
                    {
                        // 查询材料记录
                        var material = _dbContext.Set<WmsMaterial>().Where(m => m.IsDel == 1 && m.MaterialId == invmovedetail.MaterialId).FirstOrDefault();
                        if (material == null)
                        {
                            flag2 = false;
                            break;
                        }

                        list2.Add(new PrintListItem.Inventorymove
                        {
                            InventorymoveId = invmove.InventorymoveId,
                            MoveDetailId = invmovedetail.MoveDetailId,
                            MaterialNo = material.MaterialNo,
                            MaterialName = material.MaterialName,
                            Status = invmovedetail.Status,
                            PlanQty = invmovedetail.PlanQty,
                            ActQty = invmovedetail.ActQty,
                            Remark = invmovedetail.Remark,
                            AuditinTime = invmovedetail.AuditinTime,
                            AName = invmovedetail.AuditinByUser.UserNickname,
                            CName = invmovedetail.CreateByUser.UserNickname,
                            UName = invmovedetail.ModifiedByUser.UserNickname,
                            CreateDate = invmovedetail.CreateDate,
                            ModifiedDate = invmovedetail.ModifiedDate
                        });
                    }
                }
                // 返回包含多个部分的 JSON 字符串
                return (flag1, flag2, list2).JilToJson();
            }
            catch (Exception ex)
            {
                var _nlog = ServiceResolve.Resolve<ILogUtil>();
                _nlog.Error("查询材料信息失败" + ex.Message);
                return (false, ex.Message).JilToJson();
            }
        }
    }
}
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

            // ʹ�� Newtonsoft.Json �� System.Text.Json ���� JSON ���л�
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }


        public async Task<bool> AuditinAsync(long userId, long InventorymoveId)
        {
            // ʹ�� DbContext ��ʼ����
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    // ��ѯ inventorymove ��¼
                    var invmove = await _dbContext.Set<WmsInventorymove>().FindAsync(InventorymoveId);
                    if (invmove == null)
                    {
                        throw new Exception("Inventorymove not found.");
                    }

                    // ��ѯ inventorymove detail ��¼
                    var invmovedetailList = await _dbContext.Set<WmsInvmovedetail>().Where(c => c.InventorymoveId == InventorymoveId).ToListAsync();

                    foreach (var invmovedetail in invmovedetailList)
                    {
                        // ��ѯĿ�����¼
                        var targetInventory = await _dbContext.Set<WmsInventory>().Where(i => i.MaterialId == invmovedetail.MaterialId && i.StoragerackId == invmove.AimStoragerackId).FirstOrDefaultAsync();
                        if (targetInventory == null)
                        {
                            // ���û��Ŀ�����¼��������¼�¼
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
                            // �����Ŀ�����¼�������������
                            targetInventory.Qty += invmovedetail.ActQty;
                        }

                        // ���¿���¼
                        await _dbContext.SaveChangesAsync();

                        // ���� inventorymove detail ��¼״̬
                        invmovedetail.Status = (byte)StockInStatus.egis;
                        invmovedetail.AuditinId = userId;
                        invmovedetail.AuditinTime = DateTimeExt.DateTime;
                        invmovedetail.ModifiedBy = userId;
                        invmovedetail.ModifiedDate = DateTimeExt.DateTime;
                        await _dbContext.SaveChangesAsync();
                    }

                    // ���� inventorymove ����״̬
                    invmove.Status = (byte)StockInStatus.egis;
                    invmove.ModifiedBy = userId;
                    invmove.ModifiedDate = DateTimeExt.DateTime;
                    await _dbContext.SaveChangesAsync();

                    // �ύ����
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    // �ع�����
                    transaction.Rollback();
                    var _nlog = ServiceResolve.Resolve<ILogUtil>();
                    _nlog.Error("���ʧ��" + ex);
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
                // ��ѯ inventorymove ��¼
                var invmove = _dbContext.Set<WmsInventorymove>().Where(s => s.IsDel == 1 && s.InventorymoveId == InventorymoveId).FirstOrDefault();
                if (invmove == null)
                {
                    flag1 = false;
                }
                else
                {
                    // ��ѯ inventorymove detail ��¼
                    var invmovedetailList = _dbContext.Set<WmsInvmovedetail>().Where(s => s.IsDel == 1 && s.InventorymoveId == InventorymoveId).ToList();
                    foreach (var invmovedetail in invmovedetailList)
                    {
                        // ��ѯ���ϼ�¼
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
                // ���ذ���������ֵ� JSON �ַ���
                return (flag1, flag2, list2).JilToJson();
            }
            catch (Exception ex)
            {
                var _nlog = ServiceResolve.Resolve<ILogUtil>();
                _nlog.Error("��ѯ������Ϣʧ��" + ex.Message);
                return (false, ex.Message).JilToJson();
            }
        }
    }
}
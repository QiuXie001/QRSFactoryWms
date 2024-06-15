using IRepository;
using IServices;
using Microsoft.AspNetCore.Hosting;
using DB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qiu.Core.Dto;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IServices.Wms;
using IRepository.Wms;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Qiu.Utils.Files;
using DB.Dto;
using Qiu.NetCore.DI;
using Qiu.Utils.Log;

namespace Services
{
    public class Wms_StockinService : BaseService<WmsStockin>, IWms_StockinService
    {
        private readonly IWms_StockinRepository _repository;
        private readonly IWms_StockindetailRepository _detail;
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly IWms_InventoryRepository _inventory;
        private readonly IWms_InventoryrecordRepository _inventoryrecord;
        private readonly IWebHostEnvironment _env;

        public Wms_StockinService(
            QrsfactoryWmsContext dbContext,
            IWms_InventoryRepository inventoryRepository,
            IWms_InventoryrecordRepository inventoryrecordRepository,
            IWms_StockindetailRepository detail,
            IWebHostEnvironment env,
            IWms_StockinRepository repository) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
            _detail = detail;
            _inventory = inventoryRepository;
            _inventoryrecord = inventoryrecordRepository;
            _env = env;
        }

        public async Task<string> PageListAsync(PubParams.StockInBootstrapParams bootstrap)
        {
            int totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<WmsStockin>()
                .Include(s => s.Supplier)
                .Include(s => s.StockInType)
                .Include(s => s.CreateByUser)
                .Include(s => s.ModifiedByUser)
                .Where(s => s.IsDel == 1 && s.StockInType.IsDel == 1 && s.CreateByUser.IsDel == 1)
                .Select(s => new
                {
                    StockInId = s.StockInId.ToString(),
                    StockInTypeName = s.StockInType.DictName,
                    StockInTypeId = s.StockInType.DictId.ToString(),
                    StockInStatus = s.StockInStatus,
                    StockInNo = s.StockInNo,
                    OrderNo = s.OrderNo,
                    SupplierId = s.Supplier.SupplierId.ToString(),
                    SupplierNo = s.Supplier.SupplierNo,
                    SupplierName = s.Supplier.SupplierName,
                    IsDel = s.IsDel,
                    Remark = s.Remark,
                    CName = s.CreateByUser.UserNickname,
                    CreateDate = s.CreateDate,
                    UName = s.ModifiedByUser.UserNickname,
                    ModifiedDate = s.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(s => s.StockInNo.Contains(bootstrap.search) || s.OrderNo.Contains(bootstrap.search));
            }

            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query = query.Where(s => s.CreateDate > bootstrap.datemin.ToDateTimeB() && s.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }

            if (!bootstrap.StockInType.IsEmpty())
            {
                query = query.Where(s => s.StockInTypeId.Contains(bootstrap.StockInType));
            }

            if (!bootstrap.StockInStatus.IsEmpty())
            {
                query = query.Where(s => s.StockInStatus == bootstrap.StockInStatus.ToByte());
            }

            if (bootstrap.order != null && bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(s => EF.Property<object>(s, bootstrap.sort));
            }
            else
            {
                query = query.OrderBy(s => EF.Property<object>(s, bootstrap.sort));
            }

            var list = await query.Skip((pageNumber - 1) * bootstrap.limit)
                                  .Take(bootstrap.limit)
                                  .ToListAsync();

            totalNumber = await query.CountAsync();

            // ʹ�� Newtonsoft.Json �� System.Text.Json ���� JSON ���л�
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }


        public string PrintList(long stockInId)
        {

            var flag1 = true;
            var flag2 = true;
            var list2 = new List<PrintListItem.StockIn>();

            try
            {
                // ��ѯ stockin ��¼
                var stockin = _dbContext.Set<WmsStockin>().Where(s => s.IsDel == 1 && s.StockInId == stockInId).FirstOrDefault();
                if (stockin == null)
                {
                    flag1 = false;
                }
                else
                {
                    // ��ѯ supplier ��¼
                    var supplier = _dbContext.Set<WmsSupplier>().Where(p => p.IsDel == 1 && p.SupplierId == stockin.SupplierId).FirstOrDefault();
                    if (supplier == null)
                    {
                        flag1 = false;
                    }
                    else
                    {
                        // ��ѯ stockindetail ��¼
                        var stockindetailList = _dbContext.Set<WmsStockindetail>().Where(s => s.IsDel == 1 && s.StockInId == stockInId).ToList();
                        foreach (var stockindetail in stockindetailList)
                        {
                            // ��ѯ material ��¼
                            var material = _dbContext.Set<WmsMaterial>().Where(m => m.IsDel == 1 && m.MaterialId == stockindetail.MaterialId).FirstOrDefault();
                            if (material == null)
                            {
                                flag2 = false;
                                break;
                            }

                            list2.Add(new PrintListItem.StockIn
                            {
                                StockInId = stockin.StockInId,
                                StockInDetailId = stockindetail.StockInDetailId,
                                MaterialNo = material.MaterialNo,
                                MaterialName = material.MaterialName,
                                Status = stockindetail.Status,
                                PlanInQty = stockindetail.PlanInQty,
                                ActInQty = stockindetail.ActInQty,
                                Remark = stockindetail.Remark,
                                AuditinTime = stockindetail.AuditinTime,
                                AName = stockindetail.AuditinByUser.UserNickname,
                                CName = stockindetail.CreateByUser.UserNickname,
                                UName = stockindetail.ModifiedByUser.UserNickname,
                                CreateDate = stockindetail.CreateDate,
                                ModifiedDate = stockindetail.ModifiedDate
                            });
                        }
                    }
                }

                // ���ذ���������ֵ� JSON �ַ���
                return (flag1, flag2, list2).JilToJson();
            }
            catch (Exception ex)
            {
                // ��¼������־
                var _nlog = ServiceResolve.Resolve<ILogUtil>();
                _nlog.Error("��ȡ�����Ϣʧ��" + ex);
                return false.JilToJson();
            }
        }

        public async Task<bool> AuditinAsync(long UserId, long stockInId)
        {
            // ʹ�� DbContext ��ʼ����
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    // ��ѯ stockindetail ��¼
                    var stockInDetailList = await _dbContext.Set<WmsStockindetail>().Where(c => c.StockInId == stockInId && c.IsDel == 1).ToListAsync();

                    foreach (var stockInDetail in stockInDetailList)
                    {
                        // ��ѯ inventory ��¼
                        var inventory = await _dbContext.Set<WmsInventory>().Where(i => i.MaterialId == stockInDetail.MaterialId && i.StoragerackId == stockInDetail.StoragerackId && i.IsDel == 1).FirstOrDefaultAsync();
                        if (inventory == null)
                        {
                            // ���û�п���¼��������¼�¼
                            inventory = new WmsInventory
                            {
                                InventoryId = PubId.SnowflakeId,
                                StoragerackId = stockInDetail.StoragerackId,
                                CreateBy = UserId,
                                Qty = stockInDetail.ActInQty,
                                MaterialId = stockInDetail.MaterialId
                            };
                            await _dbContext.Set<WmsInventory>().AddAsync(inventory);
                        }
                        else
                        {
                            // ����п���¼�������������
                            inventory.Qty += stockInDetail.ActInQty;
                        }

                        // ���¿���¼
                        await _dbContext.SaveChangesAsync();

                        // ��� inventoryrecord ��¼
                        var inventoryRecord = new WmsInventoryrecord
                        {
                            InventoryrecordId = PubId.SnowflakeId,
                            CreateBy = UserId,
                            Qty = stockInDetail.ActInQty,
                            StockInDetailId = stockInDetail.StockInDetailId
                        };
                        await _dbContext.Set<WmsInventoryrecord>().AddAsync(inventoryRecord);
                    }

                    // �޸� stockindetail ״̬
                    var stockInDetailUpdate = new WmsStockindetail
                    {
                        Status = (byte)StockInStatus.egis,
                        AuditinId = UserId,
                        AuditinTime = DateTimeExt.DateTime,
                        ModifiedBy = UserId,
                        ModifiedDate = DateTimeExt.DateTime
                    };
                    await _detail.UpdateAsync(stockInDetailUpdate);

                    // �޸� stockin ����״̬
                    var stockInUpdate = new WmsStockin
                    {
                        StockInStatus = (byte)StockInStatus.egis,
                        ModifiedBy = UserId,
                        ModifiedDate = DateTimeExt.DateTime
                    };
                    await _repository.UpdateAsync(stockInUpdate);

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
    }
}
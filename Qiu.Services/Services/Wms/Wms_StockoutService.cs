using IRepository;
using IServices;
using Microsoft.AspNetCore.Hosting;
using DB.Models;
using System;
using System.IO;
using System.Linq;
using Qiu.Core.Dto;
using Qiu.Utils.Check;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IRepository.Wms;
using IServices.Wms;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Qiu.Utils.Files;
using Qiu.NetCore.DI;
using Qiu.Utils.Log;
using DB.Dto;
using SqlSugar;

namespace Services
{
    public class Wms_StockoutService : BaseService<WmsStockout>, IWms_StockoutService
    {
        private readonly IWms_StockoutRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly IWms_StockoutdetailRepository _detail;
        private readonly IWms_InventoryRepository _inventory;

        public Wms_StockoutService(QrsfactoryWmsContext dbContext,
            IWms_StockoutRepository repository,
            IWebHostEnvironment env,
            IWms_StockoutdetailRepository detail,
            IWms_InventoryRepository inventory
            ) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
            _env = env;
            _detail = detail;
            _inventory = inventory;
        }

        public async Task<string> PageListAsync(PubParams.StockOutBootstrapParams bootstrap)
        {
            int totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<WmsStockout>()
                .Include(s => s.Customer)
                .Include(s => s.StockOutType)
                .Include(s => s.CreateByUser)
                .Include(s => s.ModifiedByUser)
                .Where(s => s.IsDel == 1 && s.StockOutType.IsDel == 1 && s.CreateByUser.IsDel == 1)
                .Select(s => new
                {
                    StockOutId = s.StockOutId.ToString(),
                    StockOutTypeName = s.StockOutType.DictName,
                    StockOutTypeId = s.StockOutType.DictId.ToString(),
                    StockOutStatus = s.StockOutStatus,
                    StockOutNo = s.StockOutNo,
                    OrderNo = s.OrderNo,
                    CustomerId = s.Customer.CustomerId.ToString(),
                    CustomerNo = s.Customer.CustomerNo,
                    CustomerName = s.Customer.CustomerName,
                    IsDel = s.IsDel,
                    Remark = s.Remark,
                    CName = s.CreateByUser.UserNickname,
                    CreateDate = s.CreateDate,
                    UName = s.ModifiedByUser.UserNickname,
                    ModifiedDate = s.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(s => s.StockOutNo.Contains(bootstrap.search) || s.OrderNo.Contains(bootstrap.search));
            }

            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query = query.Where(s => s.CreateDate > bootstrap.datemin.ToDateTimeB() && s.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }

            if (!bootstrap.StockOutType.IsEmpty())
            {
                query = query.Where(s => s.StockOutTypeId.Contains(bootstrap.StockOutType));
            }

            if (!bootstrap.StockOutStatus.IsEmpty())
            {
                query = query.Where(s => s.StockOutStatus == bootstrap.StockOutStatus.ToByte());
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

        public string PrintList(long stockOutId)
        {
            var flag1 = true;
            var flag2 = true;
            var list2 = new List<PrintListItem.StockOut>();

            try
            {
                // ��ѯ stockout ��¼
                var stockout = _dbContext.Set<WmsStockout>().Where(s => s.IsDel == 1 && s.StockOutId == stockOutId).FirstOrDefault();
                if (stockout == null)
                {
                    flag1 = false;
                }
                else
                {
                    // ��ѯ customer ��¼
                    var customer = _dbContext.Set<WmsCustomer>().Where(p => p.IsDel == 1 && p.CustomerId == stockout.CustomerId).FirstOrDefault();
                    if (customer == null)
                    {
                        flag1 = false;
                    }
                    else
                    {
                        // ��ѯ stockoutdetail ��¼
                        var stockoutdetailList = _dbContext.Set<WmsStockoutdetail>().Where(s => s.IsDel == 1 && s.StockOutId == stockOutId).ToList();
                        foreach (var stockoutdetail in stockoutdetailList)
                        {
                            // ��ѯ material ��¼
                            var material = _dbContext.Set<WmsMaterial>().Where(m => m.IsDel == 1 && m.MaterialId == stockoutdetail.MaterialId).FirstOrDefault();
                            if (material == null)
                            {
                                flag2 = false;
                                break;
                            }

                            list2.Add(new PrintListItem.StockOut
                            {
                                StockOutId = stockout.StockOutId,
                                StockOutDetailId = stockoutdetail.StockOutDetailId,
                                MaterialNo = material.MaterialNo,
                                MaterialName = material.MaterialName,
                                Status = stockoutdetail.Status,
                                PlanInQty = stockoutdetail.PlanOutQty,
                                ActInQty = stockoutdetail.ActOutQty,
                                Remark = stockoutdetail.Remark,
                                AuditinTime = stockoutdetail.AuditinTime,
                                AName = stockoutdetail.AuditinByUser.UserNickname,
                                CName = stockoutdetail.CreateByUser.UserNickname,
                                UName = stockoutdetail.ModifiedByUser.UserNickname,
                                CreateDate = stockoutdetail.CreateDate,
                                ModifiedDate = stockoutdetail.ModifiedDate
                            });
                        }
                    }
                }

                // ��ȡ HTML ģ��
                var html = FileUtil.ReadFileFromPath(Path.Combine(_env.WebRootPath, "upload", "StockOut.html"));

                // ���ذ���������ֵ� JSON �ַ���
                return (flag1, flag2, list2, html).JilToJson();
            }
            catch (Exception ex)
            {
                var _nlog = ServiceResolve.Resolve<ILogUtil>();
                _nlog.Error("��ȡ������Ϣʧ��" + ex);
                return false.JilToJson();
            }
        }

        public async Task<bool> AuditinAsync(long userId, long stockOutId)
        {
            // ʹ�� DbContext ��ʼ����
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    // ��ѯ stockoutdetail ��¼
                    var stockOutDetailList = await _dbContext.Set<WmsStockoutdetail>().Where(c => c.StockOutId == stockOutId && c.IsDel == 1).ToListAsync();

                    foreach (var stockOutDetail in stockOutDetailList)
                    {
                        // ��ѯ inventory ��¼
                        var inventory = await _dbContext.Set<WmsInventory>().Where(i => i.MaterialId == stockOutDetail.MaterialId && i.StoragerackId == stockOutDetail.StoragerackId && i.IsDel == 1).FirstOrDefaultAsync();
                        if (inventory == null)
                        {
                            // ���û�п���¼���򷵻ش���
                            throw new Exception("����¼ not found.");
                        }

                        if (inventory.Qty < stockOutDetail.ActOutQty)
                        {
                            // ���������������Գ��⣬�򷵻ش���
                            throw new Exception("��治��.");
                        }

                        // ���¿������
                        inventory.Qty -= stockOutDetail.ActOutQty;
                        inventory.ModifiedBy = userId;
                        inventory.ModifiedDate = DateTimeExt.DateTime;
                        await _dbContext.SaveChangesAsync();
                    }

                    // �޸� stockoutdetail ״̬
                    var stockOutDetailUpdate = new WmsStockoutdetail
                    {
                        Status = (byte)StockInStatus.egis,
                        AuditinId = userId,
                        AuditinTime = DateTimeExt.DateTime,
                        ModifiedBy = userId,
                        ModifiedDate = DateTimeExt.DateTime
                    };
                    await _detail.UpdateAsync(stockOutDetailUpdate);

                    // �޸� stockout ����״̬
                    var stockOutUpdate = new WmsStockout
                    {
                        StockOutStatus = (byte)StockInStatus.egis,
                        ModifiedBy = userId,
                        ModifiedDate = DateTimeExt.DateTime
                    };
                    await _repository.UpdateAsync(stockOutUpdate);

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
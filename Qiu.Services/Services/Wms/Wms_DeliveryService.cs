using IRepository;
using IServices;
using DB.Models;
using System;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IServices.Wms;
using IRepository.Wms;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Qiu.NetCore.DI;
using Qiu.Utils.Log;

namespace Services
{
    public class Wms_DeliveryService : BaseService<WmsDelivery>, IWms_DeliveryService
    {
        private readonly IWms_DeliveryRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_DeliveryService(IWms_DeliveryRepository repository,
            QrsfactoryWmsContext dbContext
            ) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            int totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<WmsDelivery>()
                .Include(d => d.Stockout)
                .Include(d => d.Carrier)
                .Include(d => d.CreateByUser)
                .Include(d => d.ModifiedByUser)
                .Where(d => d.IsDel == 1)
                .Select(d => new
                {
                    DeliveryId = d.DeliveryId.ToString(),
                    d.DeliveryDate,
                    d.TrackingNo,
                    StockOutNo = d.Stockout.StockOutNo,
                    CarrierNo = d.Carrier.CarrierNo,
                    CarrierName = d.Carrier.CarrierName,
                    CarrierPerson = d.Carrier.CarrierPerson,
                    Tel = d.Carrier.Tel,
                    d.IsDel,
                    d.Remark,
                    CName = d.CreateByUser.UserNickname,
                    d.CreateDate,
                    UName = d.ModifiedByUser.UserNickname,
                    d.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(d => d.TrackingNo.Contains(bootstrap.search) || d.StockOutNo.Contains(bootstrap.search));
            }

            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query = query.Where(d => d.CreateDate > bootstrap.datemin.ToDateTimeB() && d.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }

            if (bootstrap.order != null && bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(d => EF.Property<object>(d, bootstrap.sort));
            }
            else
            {
                query = query.OrderBy(d => EF.Property<object>(d, bootstrap.sort));
            }

            var list = await query.Skip((pageNumber - 1) * bootstrap.limit)
                                  .Take(bootstrap.limit)
                                  .ToListAsync();

            totalNumber = await query.CountAsync();

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }


        public async Task<(bool,string)> DeliveryAsync(WmsDelivery delivery)
        {
            // 使用 DbContext 开始事务
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    // 插入新的 WmsDelivery 记录
                    await _repository.InsertAsync(delivery);

                    // 更新 WmsStockout 记录
                    var stockout = await _dbContext.Set<WmsStockout>().FindAsync(delivery.StockOutId);
                    if (stockout == null)
                    {
                        return (false, PubConst.Import1);
                    }

                    stockout.StockOutStatus = (byte)StockInStatus.delivery;
                    stockout.ModifiedBy = delivery.ModifiedBy;
                    stockout.ModifiedDate = delivery.ModifiedDate;

                    await _dbContext.SaveChangesAsync();

                    // 提交事务
                    transaction.Commit();
                    return (true,PubConst.Import2);
                }
                catch (Exception ex)
                {
                    // 回滚事务
                    transaction.Rollback();
                    var _nlog = ServiceResolve.Resolve<ILogUtil>();
                    _nlog.Error("导入客户信息失败"+ex);
                    return (false, PubConst.Import3);
                }
            }
        }

    }
}
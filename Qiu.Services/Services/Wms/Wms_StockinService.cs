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

        public Task<bool> Auditin(long UserId, long stockInId)
        {
            throw new NotImplementedException();
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

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }


        public Task<string> PrintListAsync(string stockInId)
        {
            throw new NotImplementedException();
        }
    }
}
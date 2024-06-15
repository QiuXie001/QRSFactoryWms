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

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }


        public Task<bool> Auditin(long userId, long stockOutId)
        {
            throw new NotImplementedException();
        }

        public Task<string> PrintList(string stockInId)
        {
            throw new NotImplementedException();
        }
    }
}
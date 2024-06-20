using DB.Models;
using IRepository.Wms;
using IServices.Wms;
using Microsoft.EntityFrameworkCore;
using Qiu.Utils.Extensions;
using Qiu.Utils.Table;
using System.Text.Json;

namespace Services
{
    public class Wms_WarehouseService : BaseService<WmsWarehouse>, IWms_WarehouseService
    {
        private readonly IWms_WarehouseRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_WarehouseService(QrsfactoryWmsContext dbContext, IWms_WarehouseRepository repository) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            int totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<WmsWarehouse>()
                .Include(w => w.CreateByUser)
                .Include(w => w.ModifiedByUser)
                .Where(w => w.IsDel == 1)
                .Select(w => new
                {
                    WarehouseId = w.WarehouseId.ToString(),
                    WarehouseNo = w.WarehouseNo,
                    WarehouseName = w.WarehouseName,
                    IsDel = w.IsDel,
                    Remark = w.Remark,
                    CName = w.CreateByUser.UserNickname,
                    CreateDate = w.CreateDate,
                    UName = w.ModifiedByUser.UserNickname,
                    ModifiedDate = w.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(w => w.WarehouseName.Contains(bootstrap.search) || w.WarehouseNo.Contains(bootstrap.search));
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
    }
}
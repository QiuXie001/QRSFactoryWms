using DB.Models;
using IRepository.Wms;
using IServices.Wms;
using Microsoft.EntityFrameworkCore;
using Qiu.Utils.Extensions;
using Qiu.Utils.Table;
using System.Text.Json;

namespace Services
{
    public class Wms_StoragerackService : BaseService<WmsStoragerack>, IWms_StoragerackService
    {
        private readonly IWms_StoragerackRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_StoragerackService(
            QrsfactoryWmsContext dbContext,
            IWms_StoragerackRepository repository) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            int totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<WmsStoragerack>()
                .Include(s => s.Reservoirarea)
                .Include(s => s.Reservoirarea.Warehouse)
                .Include(s => s.CreateByUser)
                .Include(s => s.ModifiedByUser)
                .Where(s => s.IsDel == 1 && s.Reservoirarea.IsDel == 1 && s.Reservoirarea.Warehouse.IsDel == 1)
                .Select(s => new
                {
                    StorageRackId = s.StorageRackId.ToString(),
                    StorageRackNo = s.StorageRackNo,
                    StorageRackName = s.StorageRackName,
                    ReservoirAreaId = s.Reservoirarea.ReservoirAreaId,
                    ReservoirAreaName = s.Reservoirarea.ReservoirAreaName,
                    WarehouseId = s.Reservoirarea.Warehouse.WarehouseId,
                    WarehouseName = s.Reservoirarea.Warehouse.WarehouseName,
                    IsDel = s.IsDel,
                    Remark = s.Remark,
                    CName = s.CreateByUser.UserNickname,
                    CreateDate = s.CreateDate,
                    UName = s.ModifiedByUser.UserNickname,
                    ModifiedDate = s.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(s => s.WarehouseName.Contains(bootstrap.search));
            }

            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query = query.Where(s => s.CreateDate > bootstrap.datemin.ToDateTimeB() && s.CreateDate <= bootstrap.datemax.ToDateTimeE());
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

        public async Task<Dictionary<long, string>> GetStorageRackList()
        {
            var result = await _dbContext.Set<WmsStoragerack>()
                .Where(r => r.IsDel == 1)
                .ToDictionaryAsync(r => r.StorageRackId, r => r.StorageRackName);
            return result;
        }
    }
}
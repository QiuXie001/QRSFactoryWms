using IRepository;
using IServices;
using DB.Models;
using System;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Table;
using IRepository.Wms;
using IServices.Wms;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Services
{
    public class Wms_ReservoirareaService : BaseService<WmsReservoirarea>, IWms_ReservoirareaService
    {
        private readonly IWms_ReservoirareaRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_ReservoirareaService(QrsfactoryWmsContext dbContext, IWms_ReservoirareaRepository repository) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            int totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<WmsReservoirarea>()
                .Include(r => r.CreateByUser)
                .Include(r => r.ModifiedByUser)
                .Include(r => r.Warehouse)
                .Where(r => r.IsDel == 1)
                .Select(r => new
                {
                    ReservoirAreaId = r.ReservoirAreaId.ToString(),
                    ReservoirAreaNo = r.ReservoirAreaNo,
                    ReservoirAreaName = r.ReservoirAreaName,
                    WarehouseNo = r.Warehouse.WarehouseNo,
                    WarehouseName = r.Warehouse.WarehouseName,
                    IsDel = r.IsDel,
                    Remark = r.Remark,
                    CName = r.CreateByUser.UserNickname,
                    CreateDate = r.CreateDate,
                    UName = r.ModifiedByUser.UserNickname,
                    ModifiedDate = r.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(r => r.WarehouseName.Contains(bootstrap.search) || r.WarehouseNo.Contains(bootstrap.search));
            }

            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query = query.Where(r => r.CreateDate > bootstrap.datemin.ToDateTimeB() && r.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }

            if (bootstrap.order != null && bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(r => EF.Property<object>(r, bootstrap.sort));
            }
            else
            {
                query = query.OrderBy(r => EF.Property<object>(r, bootstrap.sort));
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
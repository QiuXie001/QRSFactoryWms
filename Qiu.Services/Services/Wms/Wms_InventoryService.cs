using IRepository;
using IServices;
using DB.Models;
using System;
using Qiu.Core.Dto;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Table;
using IServices.Wms;
using IRepository.Wms;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Services
{
    public class Wms_InventoryService : BaseService<WmsInventory>, IWms_InventoryService
    {
        private readonly IWms_InventoryRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_InventoryService(
            QrsfactoryWmsContext dbContext,
            IWms_InventoryRepository repository) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public async Task<string> PageListAsync(PubParams.InventoryBootstrapParams bootstrap)
        {
            int totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<WmsInventory>()
                .Include(i => i.Material)
                .Include(i => i.Storagerack)
                .Include(i => i.CreateByUser)
                .Include(i => i.ModifiedByUser)
                .Where(i => i.IsDel == 1 && i.Material.IsDel == 1 && i.Storagerack.IsDel == 1 && i.CreateByUser.IsDel == 1)
                .Select(i => new
                {
                    InventoryId = i.InventoryId.ToString(),
                    i.Qty,
                    MaterialId = i.Material.MaterialId.ToString(),
                    MaterialNo = i.Material.MaterialNo,
                    MaterialName = i.Material.MaterialName,
                    SafeQty = i.Material.Qty,
                    StorageRackId = i.Storagerack.StorageRackId.ToString(),
                    StorageRackNo = i.Storagerack.StorageRackNo,
                    StorageRackName = i.Storagerack.StorageRackName,
                    i.IsDel,
                    i.Remark,
                    CName = i.CreateByUser.UserNickname,
                    i.CreateDate,
                    UName = i.ModifiedByUser.UserNickname,
                    i.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(i => i.MaterialNo.Contains(bootstrap.search) || i.MaterialName.Contains(bootstrap.search));
            }

            if (!bootstrap.StorageRackId.IsEmpty())
            {
                query = query.Where(i => i.StorageRackId == bootstrap.StorageRackId);
            }

            if (!bootstrap.MaterialId.IsEmpty())
            {
                query = query.Where(i => i.MaterialId == bootstrap.MaterialId);
            }

            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query = query.Where(i => i.CreateDate > bootstrap.datemin.ToDateTimeB() && i.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }

            if (bootstrap.order != null && bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(i => EF.Property<object>(i, bootstrap.sort));
            }
            else
            {
                query = query.OrderBy(i => EF.Property<object>(i, bootstrap.sort));
            }

            var list = await query.Skip((pageNumber - 1) * bootstrap.limit)
                                  .Take(bootstrap.limit)
                                  .ToListAsync();

            totalNumber = await query.CountAsync();

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }


        public Task<string> SearchInventory(PubParams.InventoryBootstrapParams bootstrap)
        {
            throw new NotImplementedException();
        }
    }
}
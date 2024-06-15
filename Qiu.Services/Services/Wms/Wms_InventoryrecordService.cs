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
    public class Wms_InventoryrecordService : BaseService<WmsInventoryrecord>, IWms_InventoryrecordService
    {
        private readonly IWms_InventoryrecordRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_InventoryrecordService(
            QrsfactoryWmsContext dbContext,
            IWms_InventoryrecordRepository repository) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public async Task<string> PageListAsync(PubParams.InventoryBootstrapParams bootstrap)
        {
            int totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<WmsInventoryrecord>()
                .Include(ir => ir.Stockindetail)
                .Include(ir => ir.Stockindetail.Material)
                .Include(ir => ir.CreateByUser)
                .Include(ir => ir.ModifiedByUser)
                .Include(ir => ir.Stockindetail.Stockin)
                .Include(ir => ir.Stockindetail.Storagerack)
                .Where(ir => ir.IsDel == 1 && ir.Stockindetail.Material.IsDel == 1 && ir.CreateByUser.IsDel == 1 && ir.Stockindetail.Stockin.IsDel == 1 && ir.Stockindetail.Storagerack.IsDel == 1)
                .Select(ir => new
                {
                    InventoryrecordId = ir.InventoryrecordId.ToString(),
                    StockInNo = ir.Stockindetail.Stockin.StockInNo,
                    ir.Qty,
                    MaterialNo = ir.Stockindetail.Material.MaterialNo,
                    MaterialName = ir.Stockindetail.Material.MaterialName,
                    StorageRackNo = ir.Stockindetail.Storagerack.StorageRackNo,
                    StorageRackName = ir.Stockindetail.Storagerack.StorageRackName,
                    ir.IsDel,
                    ir.Remark,
                    CName = ir.CreateByUser.UserNickname,
                    ir.CreateDate,
                    UName = ir.ModifiedByUser.UserNickname,
                    ir.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(ir => ir.MaterialNo.Contains(bootstrap.search) || ir.MaterialName.Contains(bootstrap.search) || ir.StockInNo.Contains(bootstrap.search));
            }

            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query = query.Where(ir => ir.CreateDate > bootstrap.datemin.ToDateTimeB() && ir.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }

            if (bootstrap.order != null && bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(ir => EF.Property<object>(ir, bootstrap.sort));
            }
            else
            {
                query = query.OrderBy(ir => EF.Property<object>(ir, bootstrap.sort));
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
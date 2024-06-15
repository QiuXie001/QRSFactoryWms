using IRepository;
using IServices;
using DB.Models;
using System;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IRepository.Wms;
using IServices.Wms;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Qiu.Utils.Log;
using Qiu.Utils.Excel;

namespace Services
{
    public class Wms_MaterialService : BaseService<WmsMaterial>, IWms_MaterialService
    {
        private readonly IWms_MaterialRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_MaterialService(QrsfactoryWmsContext dbContext, IWms_MaterialRepository repository) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            int totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<WmsMaterial>()
                .Include(m => m.MaterialType)
                .Include(m => m.Unit)
                .Include(m => m.Storagerack)
                .Include(m => m.Reservoirarea)
                .Include(m => m.Warehouse)
                .Include(m => m.CreateByUser)
                .Include(m => m.ModifiedByUser)
                .Where(m => m.IsDel == 1 && m.MaterialType.IsDel == 1 && m.Storagerack.IsDel == 1 && m.Reservoirarea.IsDel == 1 && m.Warehouse.IsDel == 1 && m.CreateByUser.IsDel == 1)
                .Select(m => new
                {
                    MaterialId = m.MaterialId.ToString(),
                    MaterialNo = m.MaterialNo,
                    MaterialName = m.MaterialName,
                    StorageRackNo = m.Storagerack.StorageRackNo,
                    StorageRackName = m.Storagerack.StorageRackName,
                    ReservoirAreaNo = m.Reservoirarea.ReservoirAreaNo,
                    ReservoirAreaName = m.Reservoirarea.ReservoirAreaName,
                    WarehouseNo = m.Warehouse.WarehouseNo,
                    WarehouseName = m.Warehouse.WarehouseName,
                    MaterialTypeName = m.MaterialType.DictName,
                    UnitName = m.Unit.DictName,
                    Qty = m.Qty,
                    ExpiryDate = m.ExpiryDate,
                    IsDel = m.IsDel,
                    Remark = m.Remark,
                    CName = m.CreateByUser.UserNickname,
                    CreateDate = m.CreateDate,
                    UName = m.ModifiedByUser.UserNickname,
                    ModifiedDate = m.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(m => m.MaterialNo.Contains(bootstrap.search) || m.MaterialName.Contains(bootstrap.search));
            }

            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query = query.Where(m => m.CreateDate > bootstrap.datemin.ToDateTimeB() && m.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }

            if (bootstrap.order != null && bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(m => EF.Property<object>(m, bootstrap.sort));
            }
            else
            {
                query = query.OrderBy(m => EF.Property<object>(m, bootstrap.sort));
            }

            var list = await query.Skip((pageNumber - 1) * bootstrap.limit)
                                  .Take(bootstrap.limit)
                                  .ToListAsync();

            totalNumber = await query.CountAsync();

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }


        public async Task<byte[]> ExportListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            bootstrap.sort = "CreateDate";
            bootstrap.order = "desc";

            // 使用 DbContext 开始事务
            // 由于这个方法没有事务逻辑，所以不需要事务

            var query = _dbContext.Set<WmsMaterial>()
                .Include(s => s.MaterialType)
                .Include(s => s.Unit)
                .Include(s => s.Storagerack)
                .Include(s => s.Reservoirarea)
                .Include(s => s.Warehouse)
                .Include(s => s.CreateByUser)
                .Include(s => s.ModifiedByUser)
                .Where(s => s.IsDel == 1 && s.MaterialType.IsDel == 1 && s.Unit.IsDel == 1 && s.Storagerack.IsDel == 1 && s.Reservoirarea.IsDel == 1 && s.Warehouse.IsDel == 1 && s.CreateByUser.IsDel == 1);

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

            var list = await query.ToListAsync();
            var buffer = NpoiUtil.Export(list, ExcelVersion.V2007);
            return buffer;
        }


    }
}
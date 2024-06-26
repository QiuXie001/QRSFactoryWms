using DB.Models;
using IRepository.Wms;
using IServices.Wms;
using Microsoft.EntityFrameworkCore;
using Qiu.Utils.Excel;
using Qiu.Utils.Extensions;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using System.Text.Json;

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
                .Where(m => m.IsDel == 1 && m.MaterialType.IsDel == 1 && m.Storagerack.IsDel == 1 && m.Reservoirarea.IsDel == 1 && m.Warehouse.IsDel == 1)
                .Select(m => new
                {
                    MaterialId = m.MaterialId.ToString(),
                    MaterialNo = m.MaterialNo,
                    MaterialName = m.MaterialName,
                    StorageRackId = m.Storagerack.StorageRackId,
                    StorageRackName = m.Storagerack.StorageRackName,
                    ReservoirAreaId = m.Reservoirarea.ReservoirAreaId,
                    ReservoirAreaName = m.Reservoirarea.ReservoirAreaName,
                    WarehouseId = m.Warehouse.WarehouseId,
                    WarehouseName = m.Warehouse.WarehouseName,
                    MaterialTypeId = m.MaterialType.DictId,
                    MaterialTypeName = m.MaterialType.DictName,
                    UnitId = m.Unit.DictId,
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

            // ʹ�� Newtonsoft.Json �� System.Text.Json ���� JSON ���л�
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }


        public async Task<byte[]> ExportListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            bootstrap.sort = "CreateDate";
            bootstrap.order = "desc";

            // ʹ�� DbContext ��ʼ����
            // �����������û�������߼������Բ���Ҫ����

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

        public async Task<Dictionary<long, string>> GetMaterialList()
        {
            var result = await _dbContext.Set<WmsMaterial>()
                .Where(r => r.IsDel == 1)
                .ToDictionaryAsync(r => r.MaterialId, r => r.MaterialName);
            return result;

        }
    }
}
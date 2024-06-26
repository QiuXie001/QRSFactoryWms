using DB.Models;
using IRepository.Wms;
using IServices.Wms;
using Microsoft.EntityFrameworkCore;
using Qiu.Utils.Extensions;
using Qiu.Utils.Table;
using System.Text.Json;

namespace Services
{
    public class Wms_SupplierService : BaseService<WmsSupplier>, IWms_SupplierService
    {
        private readonly IWms_SupplierRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_SupplierService(QrsfactoryWmsContext dbContext, IWms_SupplierRepository repository) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            int totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<WmsSupplier>()
                .Include(s => s.CreateByUser)
                .Include(s => s.ModifiedByUser)
                .Where(s => s.IsDel == 1)
                .Select(s => new
                {
                    SupplierId = s.SupplierId.ToString(),
                    SupplierNo = s.SupplierNo,
                    SupplierName = s.SupplierName,
                    Address = s.Address,
                    SupplierPerson = s.SupplierPerson,
                    SupplierLevel = s.SupplierLevel,
                    Tel = s.Tel,
                    Email = s.Email,
                    IsDel = s.IsDel,
                    Remark = s.Remark,
                    CName = s.CreateByUser.UserNickname,
                    CreateDate = s.CreateDate,
                    UName = s.ModifiedByUser.UserNickname,
                    ModifiedDate = s.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(s => s.SupplierName.Contains(bootstrap.search) || s.SupplierNo.Contains(bootstrap.search));
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

        public async Task<Dictionary<long, string>> GetSupplierList()
        {
            var result = await _dbContext.Set<WmsSupplier>()
                .Where(r => r.IsDel == 1)
                .ToDictionaryAsync(r => r.SupplierId, r => r.SupplierName);
            return result;
        }
    }
}
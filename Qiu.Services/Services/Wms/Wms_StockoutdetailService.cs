using IRepository;
using IServices;
using DB.Models;
using Qiu.Utils.Json;
using Qiu.Utils.Table;
using IRepository.Wms;
using IServices.Wms;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Services
{
    public class Wms_StockoutdetailService : BaseService<WmsStockoutdetail>, IWms_StockoutdetailService
    {
        private readonly IWms_StockoutdetailRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_StockoutdetailService(IWms_StockoutdetailRepository repository,
            QrsfactoryWmsContext dbContext
            ) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<string> PageListAsync(string pid)
        {
            var query = _dbContext.Set<WmsStockoutdetail>()
                .Include(d => d.Material)
                .Include(d => d.Stockout)
                .Include(d => d.Storagerack)
                .Include(d => d.CreateByUser)
                .Include(d => d.ModifiedByUser)
                .Include(d => d.AuditinByUser)
                .Where(d => d.IsDel == 1 && d.Material.IsDel == 1 && d.Stockout.IsDel == 1 && d.Storagerack.IsDel == 1)
                .Select(d => new
                {
                    StockOutId = d.Stockout.StockOutId.ToString(),
                    StockOutDetailId = d.StockOutDetailId.ToString(),
                    MaterialNo = d.Material.MaterialNo,
                    MaterialName = d.Material.MaterialName,
                    StorageRackNo = d.Storagerack.StorageRackNo,
                    StorageRackName = d.Storagerack.StorageRackName,
                    Status = d.Status,
                    PlanOutQty = d.PlanOutQty,
                    ActOutQty = d.ActOutQty,
                    IsDel = d.IsDel,
                    Remark = d.Remark,
                    AuditinTime = d.AuditinTime,
                    AName = d.AuditinByUser.UserNickname,
                    CName = d.CreateByUser.UserNickname,
                    CreateDate = d.CreateDate,
                    UName = d.ModifiedByUser.UserNickname,
                    ModifiedDate = d.ModifiedDate
                });

            query = query.Where(d => d.StockOutId == pid).OrderByDescending(d => d.CreateDate);

            var list = await query.ToListAsync();

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = list, total = list.Count() });
        }
    }
}
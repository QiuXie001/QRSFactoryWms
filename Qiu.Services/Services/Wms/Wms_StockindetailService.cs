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
    public class Wms_StockindetailService : BaseService<WmsStockindetail>, IWms_StockindetailService
    {
        private readonly IWms_StockindetailRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_StockindetailService(QrsfactoryWmsContext dbContext, IWms_StockindetailRepository repository) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public async Task<string> PageListAsync(string pid)
        {
            var query = _dbContext.Set<WmsStockindetail>()
                .Include(d => d.Material)
                .Include(d => d.Stockin)
                .Include(d => d.Storagerack)
                .Include(d => d.CreateByUser)
                .Include(d => d.ModifiedByUser)
                .Include(d => d.AuditinByUser)
                .Where(d => d.IsDel == 1 && d.Stockin.IsDel == 1 && d.Storagerack.IsDel == 1 && d.CreateByUser.IsDel == 1)
                .Select(d => new
                {
                    StockInId = d.Stockin.StockInId.ToString(),
                    StockInDetailId = d.StockInDetailId.ToString(),
                    MaterialNo = d.Material.MaterialNo,
                    MaterialName = d.Material.MaterialName,
                    StorageRackNo = d.Storagerack.StorageRackNo,
                    StorageRackName = d.Storagerack.StorageRackName,
                    Status = d.Status,
                    PlanInQty = d.PlanInQty,
                    ActInQty = d.ActInQty,
                    IsDel = d.IsDel,
                    Remark = d.Remark,
                    AuditinTime = d.AuditinTime,
                    AName = d.AuditinByUser.UserNickname,
                    CName = d.CreateByUser.UserNickname,
                    CreateDate = d.CreateDate,
                    UName = d.ModifiedByUser.UserNickname,
                    ModifiedDate = d.ModifiedDate
                });

            query = query.Where(d => d.StockInId == pid).OrderByDescending(d => d.CreateDate);

            var list = await query.ToListAsync();

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = list, total = list.Count() });
        }

    }
}
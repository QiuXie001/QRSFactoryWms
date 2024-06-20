using DB.Models;
using IRepository.Wms;
using IServices.Wms;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Services
{
    public class Wms_InvmovedetailService : BaseService<WmsInvmovedetail>, IWms_InvmovedetailService
    {
        private readonly IWms_InvmovedetailRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_InvmovedetailService(IWms_InvmovedetailRepository repository
            ,
            QrsfactoryWmsContext dbContext) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<string> PageListAsync(string pid)
        {
            var query = _dbContext.Set<WmsInvmovedetail>()
                .Include(m => m.Material)
                .Include(m => m.Inventorymove)
                .Include(m => m.CreateByUser)
                .Include(m => m.ModifiedByUser)
                .Include(m => m.AuditinByUser)
                .Where(m => m.IsDel == 1)
                .Select(m => new
                {
                    MoveDetailId = m.MoveDetailId.ToString(),
                    InventorymoveId = m.InventorymoveId.ToString(),
                    MaterialNo = m.Material.MaterialNo,
                    MaterialName = m.Material.MaterialName,
                    Status = m.Inventorymove.Status,
                    PlanQty = m.PlanQty,
                    ActQty = m.ActQty,
                    IsDel = m.IsDel,
                    Remark = m.Remark,
                    AuditinTime = m.AuditinTime,
                    AName = m.AuditinByUser.UserNickname,
                    CName = m.CreateByUser.UserNickname,
                    CreateDate = m.CreateDate,
                    UName = m.ModifiedByUser.UserNickname,
                    ModifiedDate = m.ModifiedDate
                });

            query = query.Where(m => m.InventorymoveId == pid).OrderByDescending(m => m.CreateDate);

            var list = await query.ToListAsync();

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = list, total = list.Count() });
        }
    }
}
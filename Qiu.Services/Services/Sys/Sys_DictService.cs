using DB.Models;
using IRepository.Sys;
using IServices.Sys;
using Microsoft.EntityFrameworkCore;
using Qiu.Utils.Extensions;
using Qiu.Utils.Table;
using System.Text.Json;

namespace Services.Sys
{
    public class Sys_DictService : BaseService<SysDict>, ISys_DictService
    {
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly ISys_DictRepository _repository;
        public Sys_DictService(
            QrsfactoryWmsContext dbContext,
            ISys_DictRepository repository
            ) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            var totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<SysDict>()
                .Include(s => s.CreateByUser)
                .Include(s => s.ModifiedByUser)
                .Where(s => s.IsDel == 1 && s.CreateByUser.IsDel == 1 && s.ModifiedByUser.IsDel == 1)
                .Select(s => new
                {
                    DictId = s.DictId,
                    s.DictName,
                    s.DictNo,
                    s.IsDel,
                    s.Remark,
                    CName = s.CreateByUser.UserNickname,
                    s.CreateDate,
                    UName = s.ModifiedByUser.UserNickname,
                    s.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(s => s.DictName.Contains(bootstrap.search));
            }

            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query = query.Where(s => s.CreateDate > bootstrap.datemin.ToDateTimeB() && s.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }

            var list = await query.Skip((pageNumber - 1) * bootstrap.limit)
                                  .Take(bootstrap.limit)
                                  .ToListAsync();

            if (bootstrap.order != null && bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                list.Reverse();
            }

            totalNumber = await query.CountAsync();

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }


    }
}

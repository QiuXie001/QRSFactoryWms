using DB.Models;
using IRepository.Sys;
using IServices.Sys;
using Microsoft.EntityFrameworkCore;
using Qiu.Utils.Extensions;
using Qiu.Utils.Table;
using System.Text.Json;

namespace Services.Sys
{
    public class Sys_DeptService : BaseService<SysDept>, ISys_DeptService
    {
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly ISys_DeptRepository _repository;
        private readonly ISys_UserService _userService;
        public Sys_DeptService(
            QrsfactoryWmsContext dbContext,
            ISys_DeptRepository repository,
            ISys_UserService userService
            ) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
            _userService = userService;
        }
        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            var totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<SysDept>()
                .Include(s => s.CreateByUser)
                .Include(s => s.ModifiedByUser)
                .Where(s => s.IsDel == 1 && s.CreateByUser.IsDel == 1 && s.ModifiedByUser.IsDel == 1)
                .Select(s => new
                {
                    DeptId = s.DeptId,
                    s.DeptName,
                    s.DeptNo,
                    s.IsDel,
                    s.Remark,
                    CName = s.CreateByUser.UserNickname,
                    s.CreateDate,
                    UName = s.ModifiedByUser.UserNickname,
                    s.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(s => s.DeptName.Contains(bootstrap.search));
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
        public async Task<string> GetDeptNameById(long deptId)
        {
            var dept = await _dbContext.Set<SysDept>()
                .FirstOrDefaultAsync(d => d.DeptId == deptId && d.IsDel == 1);
            return dept?.DeptName;
        }

        public async Task<List<string>> GetDeptNameList()
        {
            return await _dbContext.Set<SysDept>()
                .Where(r => r.IsDel == 1)
                .Select(r => r.DeptName)
                .ToListAsync();
        }
    }
}

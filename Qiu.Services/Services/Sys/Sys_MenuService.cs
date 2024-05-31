using DB.Models;
using IResponsitory.Sys;
using IServices.Sys;
using Microsoft.EntityFrameworkCore;
using Qiu.Utils.Extensions;
using Qiu.Utils.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.Sys
{
    public class Sys_MenuService : BaseService<SysMenu>, ISys_MenuService
    {
        private readonly ISys_MenuResponsitory _responsitory;
        private readonly ISys_RoleMenuService _rolemenuService;
        private readonly QrsfactoryWmsContext _dbContext;
        public Sys_MenuService(
            ISys_RoleMenuService rolemenuService,
            QrsfactoryWmsContext dbContext,
            ISys_MenuResponsitory responsitory
            ) : base(responsitory)
        {
            _responsitory = responsitory;
            _dbContext = dbContext;
            _rolemenuService = rolemenuService;
        }
        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            var totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<SysMenu>()
                .Include(s => s.CreateByUser)
                .Include(s => s.ModifiedByUser)
                .Where(s => s.IsDel == 1 && s.CreateByUser.IsDel == 1 && s.ModifiedByUser.IsDel == 1)
                .Select(s => new
                {
                    MenuId = s.MenuId,
                    s.MenuName,
                    s.MenuType,
                    s.MenuUrl,
                    s.MenuParent,
                    s.MenuIcon,
                    s.Sort,
                    s.Status,
                    s.Remark,
                    CName = s.CreateByUser.UserNickname,
                    s.CreateDate,
                    UName = s.ModifiedByUser.UserNickname,
                    s.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(s => s.MenuName.Contains(bootstrap.search) || s.MenuType.Contains(bootstrap.search));
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

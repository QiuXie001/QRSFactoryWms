using DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using DB.Dto;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using IServices.Sys;
using IResponsitory.Sys;

namespace Services.Sys
{
    public class Sys_RoleService : BaseService<SysRole>, ISys_RoleService
    {
        private readonly ISys_RoleResponsitory _responsitory;
        private readonly ISys_RoleMenuService _rolemenuService;
        private readonly ISys_MenuService _menuService;
        private readonly QrsfactoryWmsContext _dbContext;
        public Sys_RoleService(
            ISys_MenuService menuServices,
            ISys_RoleMenuService rolemenuService,
            QrsfactoryWmsContext dbContext,
            ISys_RoleResponsitory responsitory
            ) : base(responsitory)
        {
            _responsitory = responsitory;
            _dbContext = dbContext;
            _rolemenuService = rolemenuService;
            _menuService = menuServices;
        }
        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            var totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<SysRole>()
                .Include(r => r.RoleMenus)
                .Include(r => r.CreateByUser)
                .Include(r => r.ModifiedByUser)
                .Where(r => r.IsDel == 1)
                .Select(r => new
                {
                    RoleId = r.RoleId.ToString(),
                    r.RoleType,
                    r.RoleName,
                    r.IsDel,
                    r.Remark,
                    CName = r.CreateByUser.UserNickname,
                    r.CreateDate,
                    MName = r.ModifiedByUser.UserNickname,
                    r.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(s => s.RoleName.Contains(bootstrap.search) || s.RoleType.Contains(bootstrap.search));
            }

            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query = query.Where(s => s.CreateDate > bootstrap.datemin.ToDateTimeB() && s.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }

            query = bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase) ?
                query.OrderByDescending(s => EF.Property<object>(s, bootstrap.sort)) :
                query.OrderBy(s => EF.Property<object>(s, bootstrap.sort));

            var list = await query.Skip((pageNumber - 1) * bootstrap.limit)
                                  .Take(bootstrap.limit)
                                  .ToListAsync();

            totalNumber = await query.CountAsync();

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }

        public async Task<List<PermissionMenu>> GetMenuAsync()
        {
            var permissionMenus = await _menuService.QueryableToListAsync(m => m.IsDel == 1 && m.MenuType == "menu" && m.Status == 1);
            var parentPermissions = permissionMenus.Where(a => a.MenuParent == -1).ToList();
            var ret = new List<PermissionMenu>();
            parentPermissions.ForEach(p =>
            {
                PermissionMenu permissionMenu = PermissionMenu.Create(p);
                permissionMenu.Children = permissionMenus
                    .Where(c => c.MenuParent == p.MenuId)
                    .Select(m => new PermissionMenu
                    {
                        Id = m.MenuId.ToString(),
                        Name = m.MenuName,
                        Icon = m.MenuIcon,
                        Url = m.MenuUrl,
                        ParentId = m.MenuParent.ToString()
                    }).ToList();
                ret.Add(permissionMenu);
            });
            return ret;
        }

        public async Task<List<PermissionMenu>> GetMenuAsync(long roleId, string menuType = "menu")
        {
            var listAll = await _dbContext.Set<SysRoleMenu>()
                .Include(rm => rm.Menu)
                .Where(rm => rm.RoleId == roleId && rm.Menu.IsDel == 1 && rm.Menu.MenuType == menuType && rm.Menu.Status == 1)
                .ToListAsync();
            var listParent = listAll.Where(s => s.Menu.MenuParent == -1).ToList();
            List<PermissionMenu> ret = new List<PermissionMenu>();
            listParent.ForEach(p =>
            {
                PermissionMenu permissionMenu = PermissionMenu.Create(p.Menu);
                permissionMenu.Children = listAll
                    .Where(c => c.Menu.MenuParent == p.MenuId)
                    .Select(m => new PermissionMenu
                    {
                        Id = m.MenuId.ToString(),
                        Name = m.Menu.MenuName,
                        Icon = m.Menu.MenuIcon,
                        Url = m.Menu.MenuUrl,
                        ParentId = m.Menu.MenuParent.ToString()
                    }).ToList();
                ret.Add(permissionMenu);
            });
            return ret;
        }

        public async Task<bool> Insert(SysRole role, long userId, string[] menuId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    role.RoleId = PubId.SnowflakeId;
                    role.CreateBy = userId;
                    role.RoleType = "#";
                    await InsertAsync(role);

                    var roleMenuList = menuId.Select(menuId => new SysRoleMenu
                    {
                        CreateBy = userId,
                        MenuId = long.Parse(menuId),
                        RoleId = role.RoleId,
                        RoleMenuId = PubId.SnowflakeId
                    }).ToList();

                    await _rolemenuService.InsertIgnoreNullColumnsBatchAsync(roleMenuList);
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> Update(SysRole role, long userId, string[] menuId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    role.ModifiedBy = userId;
                    role.ModifiedDate = DateTime.UtcNow;
                    await UpdateAsync(role);

                    // 处理角色菜单关系的更新逻辑

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
    }
}
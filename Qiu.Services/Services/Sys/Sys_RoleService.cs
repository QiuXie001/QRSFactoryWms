using DB.Dto;
using DB.Models;
using IRepository.Sys;
using IServices.Sys;
using Microsoft.EntityFrameworkCore;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using System.Text.Json;

namespace Services.Sys
{
    public class Sys_RoleService : BaseService<SysRole>, ISys_RoleService
    {
        private readonly ISys_RoleRepository _repository;
        private readonly ISys_RoleMenuService _rolemenuService;
        private readonly ISys_MenuService _menuService;
        private readonly QrsfactoryWmsContext _dbContext;
        public Sys_RoleService(
            ISys_MenuService menuServices,
            ISys_RoleMenuService rolemenuService,
            QrsfactoryWmsContext dbContext,
            ISys_RoleRepository repository
            ) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
            _rolemenuService = rolemenuService;
            _menuService = menuServices;
        }
        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            var totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<SysRole>()
                .Include(r => r.Rolemenus)
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

        public async Task<string> GetMenuAsync()
        {
            var query = _dbContext.Set<SysMenu>()
                .Include(r => r.CreateByUser)
                .Include(r => r.ModifiedByUser)
                .Where(r => r.IsDel == 1)
                .Select(r => new MenuTreeNode
                {
                    MenuId = r.MenuId,
                    MenuName = r.MenuName,
                    MenuParent = r.MenuParent,
                });

            var list = await query.ToListAsync();

            var menuTree = BuildMenuTree(list, -1);

            int totalNumber = await query.CountAsync();

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = menuTree, total = totalNumber });
        }

        //获取所有菜单
        public async Task<string> GetMenuAsync(Bootstrap.BootstrapParams bootstrap)
        {
            var totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<SysMenu>()
                .Include(r => r.CreateByUser)
                .Include(r => r.ModifiedByUser)
                .Where(r => r.IsDel == 1)
                .Select(r => new
                {
                    MenuId = r.MenuId,
                    r.MenuName,
                    r.MenuParent,
                    r.MenuType,
                    r.MenuUrl,
                    r.MenuIcon,
                    r.IsDel,
                    r.Remark,
                    r.Sort,
                    CName = r.CreateByUser.UserNickname,
                    r.CreateDate,
                    MName = r.ModifiedByUser.UserNickname,
                    r.ModifiedDate
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

        //获取菜单权限
        public async Task<List<PermissionMenu>> GetMenuAsync(long roleId, string menuType = "menu")
        {

            // 使用Include方法加载相关联的Sys_menu数据
            var roleMenus = _dbContext.SysRolemenus
                .Include(rm => rm.Menu)
                .Include(rm => rm.Role)
                .Where(rm => rm.Menu != null && rm.Menu.IsDel == 1 && rm.Menu.MenuType == menuType && rm.Menu.Status == 1 && rm.RoleId == roleId)
                .Select(rm => new SysRolemenu
                {
                    RoleMenuId = rm.RoleMenuId,
                    MenuId = rm.MenuId,
                    RoleId = rm.RoleId,
                    Menu = rm.Menu,
                    Role = rm.Role,
                    CreateByUser = rm.CreateByUser,
                    ModifiedByUser = rm.ModifiedByUser

                }).OrderBy(rm => rm.Menu.Sort)
                .ToList();

            // 过滤出所有父菜单
            var parentMenus = roleMenus.Where(rm => rm.Menu.MenuParent == -1).ToList();

            // 构建PermissionMenu树结构
            List<PermissionMenu> permissionMenus = new List<PermissionMenu>();
            foreach (var parentMenu in parentMenus)
            {
                PermissionMenu permissionMenu = PermissionMenu.Create(parentMenu.Menu);
                permissionMenu.Children = roleMenus
                    .Where(rm => rm.Menu.MenuParent == parentMenu.Menu.MenuId)
                    .Select(rm => new PermissionMenu
                    {
                        Id = rm.Menu.MenuId.ToString(),
                        Name = rm.Menu.MenuName,
                        Icon = rm.Menu.MenuIcon,
                        Url = rm.Menu.MenuUrl,
                        ParentId = rm.Menu.MenuParent.ToString()
                    })
                    .ToList();
                permissionMenus.Add(permissionMenu);
            }

            return permissionMenus;
        }

        //插入新角色并设置初始权限
        public async Task<bool> InsertRole(SysRole role, long userId, string[] menuId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    role.RoleId = PubId.SnowflakeId;

                    role.CreateBy = userId;
                    role.CreateDate = DateTime.UtcNow;

                    role.ModifiedBy = userId;
                    role.ModifiedDate = DateTime.UtcNow;

                    role.IsDel = 1;

                    await InsertAsync(role);
                    if (menuId[0] != "")
                    {
                        // 根据传入的menuId数组创建新的角色菜单关联列表
                        var roleMenuList = menuId.Select(menuId => new SysRolemenu
                        {
                            CreateBy = userId,
                            CreateDate = DateTime.UtcNow,
                            MenuId = long.Parse(menuId),
                            RoleId = role.RoleId,
                            RoleMenuId = PubId.SnowflakeId
                        }).ToList();
                        await _rolemenuService.InsertIgnoreNullColumnsBatchAsync(roleMenuList);
                    }
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
        public async Task<bool> InsertMenu(SysMenu menu, long userId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 设置菜单属性                  54042346765418496
                    menu.MenuId = PubId.SnowflakeId %1000000000000;
                    menu.CreateBy = userId;
                    menu.CreateDate = DateTime.UtcNow;
                    menu.ModifiedBy = userId;
                    menu.ModifiedDate = DateTime.UtcNow;
                    menu.IsDel = 1; // 假设新菜单默认是启用的
                    menu.Status = 1; // 假设新菜单默认是启用的

                    // 插入菜单
                    _dbContext.Add(menu);
                    await _dbContext.SaveChangesAsync();

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

        public async Task<bool> UpdateRole(RoleDto role, long userId, string[] menuId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 删除该角色现有的所有菜单关联
                    await _rolemenuService.DeleteByRoleIdAsync(role.RoleId);

                    // 根据传入的menuId数组创建新的角色菜单关联列表
                    var roleMenuList = menuId.Select(menuId => new SysRolemenu
                    {
                        CreateBy = userId,
                        CreateDate = DateTime.UtcNow,
                        MenuId = long.Parse(menuId),
                        RoleId = role.RoleId,
                        RoleMenuId = PubId.SnowflakeId
                    }).ToList();

                    // 将新的角色菜单关联列表插入到数据库中
                    await _rolemenuService.InsertIgnoreNullColumnsBatchAsync(roleMenuList);

                    var entity = await _repository.QueryableToSingleAsync(r => r.RoleId == role.RoleId);

                    entity.RoleName = role.RoleName;
                    entity.RoleType = role.RoleType;
                    entity.Remark = role.Remark;
                    entity.ModifiedBy = userId;
                    entity.ModifiedDate = DateTime.UtcNow;

                    await _repository.UpdateAsync(entity);

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

        public async Task<bool> UpdateMenu(SysMenu menu, long userId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var entity = await _menuService.QueryableToSingleAsync(m => m.MenuId == menu.MenuId);
                    menu.Status = entity.Status;
                    menu.IsDel = entity.IsDel;
                    menu.CreateBy = entity.CreateBy;
                    menu.CreateDate = entity.CreateDate;
                    menu.ModifiedBy = userId;
                    menu.ModifiedDate = DateTime.UtcNow;
                    menu.Rolemenus = null;
                    // 更新菜单
                    _dbContext.Update(menu);
                    await _dbContext.SaveChangesAsync();

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

        public async Task<bool> DisableRole(SysRole role, long userId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 更新角色的状态为禁用
                    role.IsDel = 0;
                    role.ModifiedBy = userId;
                    role.ModifiedDate = DateTime.UtcNow;

                    // 更新角色实体
                    _dbContext.Update(role);
                    await _dbContext.SaveChangesAsync();

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

        public async Task<bool> DisableMenu(SysMenu menu, long userId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 更新菜单的状态为禁用
                    menu.IsDel = 0;
                    menu.ModifiedBy = userId;
                    menu.ModifiedDate = DateTime.UtcNow;

                    // 更新菜单实体
                    _dbContext.Update(menu);
                    await _dbContext.SaveChangesAsync();

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


        public async Task<bool> DeleteRole(SysRole role)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 删除与角色关联的所有菜单关联
                    await _rolemenuService.DeleteByRoleIdAsync(role.RoleId);

                    await _dbContext.SaveChangesAsync();
                    // 删除角色本身
                    _dbContext.Remove(role);
                    await _dbContext.SaveChangesAsync();

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

        public async Task<bool> DeleteMenu(SysMenu menu)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 删除与菜单关联的所有角色关联
                    await _rolemenuService.DeleteByMenuIdAsync(menu.MenuId);

                    await _menuService.DeleteAsync(menu);

                    // 保存更改
                    await _dbContext.SaveChangesAsync();

                    // 提交事务
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    // 回滚事务
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> CheckRoleAccessToUrl(long roleId, string requestUrl)
        {
            var roleMenus = await _dbContext.Set<SysRolemenu>()
                .Include(rm => rm.Menu)
                .Where(rm => rm.RoleId == roleId && rm.Menu.MenuUrl == requestUrl && rm.Menu.IsDel == 1)
                .Select(rm => rm.Menu)
                .ToListAsync();

            return roleMenus.Any();
        }
        public async Task<string> GetRoleNameById(long roleId)
        {
            var role = await _dbContext.Set<SysRole>()
                .FirstOrDefaultAsync(r => r.RoleId == roleId && r.IsDel == 1);
            return role?.RoleName;
        }

        public async Task<Dictionary<long, string>> GetRoleList()
        {
            var result = await _dbContext.Set<SysRole>()
                .Where(r => r.IsDel == 1)
                .ToDictionaryAsync(r => r.RoleId, r => r.RoleName);
            return result;
        }

        

        private List<MenuTreeNode> BuildMenuTree(List<MenuTreeNode> allMenus, long? parentId)
        {
            return allMenus.Where(m => m.MenuParent == parentId)
                .Select(m => new MenuTreeNode
                {
                    MenuId = m.MenuId,
                    MenuName = m.MenuName,
                    MenuParent = m.MenuParent,

                    Children = BuildMenuTree(allMenus, m.MenuId)
                })
                .ToList();
        }
        private class MenuTreeNode
        {
            public long MenuId { get; set; }
            public string MenuName { get; set; }
            public long? MenuParent { get; set; }
            public List<MenuTreeNode> Children { get; set; }
        }
    }
}
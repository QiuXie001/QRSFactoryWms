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
using IRepository.Sys;
using System.Linq.Expressions;

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

            var list = await query.Skip((pageNumber - 1) * bootstrap.limit)
                                  .Take(bootstrap.limit)
                                  .ToListAsync();

            if (bootstrap.order != null && bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                list.Reverse();
            }

            totalNumber = await query.CountAsync();

            // ʹ�� Newtonsoft.Json �� System.Text.Json ���� JSON ���л�
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }
        //��ȡ���в˵�
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

            // ʹ�� Newtonsoft.Json �� System.Text.Json ���� JSON ���л�
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }

        //��ȡ�˵�Ȩ��
        public async Task<List<PermissionMenu>> GetMenuAsync(long roleId, string menuType = "menu")
        {

            // ʹ��Include���������������Sys_menu����
            var roleMenus = _dbContext.SysRolemenus
                .Include(rm => rm.Menu)
                .Include(rm => rm.Role)
                .Where(rm => rm.Menu != null && rm.Menu.IsDel == 1 && rm.Menu.MenuType == menuType && rm.Menu.Status == 1 && rm.RoleId == roleId)
                .Select(rm =>new SysRoleMenu
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

            // ���˳����и��˵�
            var parentMenus = roleMenus.Where(rm => rm.Menu.MenuParent == -1).ToList();

            // ����PermissionMenu���ṹ
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

        //�����½�ɫ�����ó�ʼȨ��
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

                    role.RoleType = "#";

                    await InsertAsync(role);

                    // ���ݴ����menuId���鴴���µĽ�ɫ�˵������б�
                    var roleMenuList = menuId.Select(menuId => new SysRoleMenu
                    {
                        CreateBy = userId,
                        CreateDate = DateTime.UtcNow,
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
        public async Task<bool> InsertMenu(SysMenu menu, long userId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // ���ò˵�����
                    menu.CreateBy = userId;
                    menu.CreateDate = DateTime.UtcNow;
                    menu.ModifiedBy = userId;
                    menu.ModifiedDate = DateTime.UtcNow;
                    menu.IsDel = 1; // �����²˵�Ĭ�������õ�

                    // ����˵�
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

        public async Task<bool> UpdateRole(SysRole role, long userId, string[] menuId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // ɾ���ý�ɫ���е����в˵�����
                    await _rolemenuService.DeleteByRoleIdAsync(role.RoleId);

                    // ���ݴ����menuId���鴴���µĽ�ɫ�˵������б�
                    var roleMenuList = menuId.Select(menuId => new SysRoleMenu
                    {
                        CreateBy = userId,
                        CreateDate = DateTime.UtcNow,
                        MenuId = long.Parse(menuId),
                        RoleId = role.RoleId,
                        RoleMenuId = PubId.SnowflakeId
                    }).ToList();

                    // ���µĽ�ɫ�˵������б���뵽���ݿ���
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

        public async Task<bool> UpdateMenu(SysMenu menu, long userId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // ���ò˵�����
                    menu.ModifiedBy = userId;
                    menu.ModifiedDate = DateTime.UtcNow;

                    // ���²˵�
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
                    // ���½�ɫ��״̬Ϊ����
                    role.IsDel = 0;
                    role.ModifiedBy = userId;
                    role.ModifiedDate = DateTime.UtcNow;

                    // ���½�ɫʵ��
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
                    // ���²˵���״̬Ϊ����
                    menu.IsDel = 0;
                    menu.ModifiedBy = userId;
                    menu.ModifiedDate = DateTime.UtcNow;

                    // ���²˵�ʵ��
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
                    // ɾ�����ɫ���������в˵�����
                    await _rolemenuService.DeleteByRoleIdAsync(role.RoleId);

                    // ɾ����ɫ����
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
                    // ɾ����˵����������н�ɫ����
                    await _rolemenuService.DeleteByMenuIdAsync(menu.MenuId);

                    // ɾ���˵�����
                    _dbContext.Remove(menu);
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


        public async Task<bool> CheckRoleAccessToUrl(long roleId, string requestUrl)
        {
            var roleMenus = await _dbContext.Set<SysRoleMenu>()
                .Include(rm => rm.Menu)
                .Where(rm => rm.RoleId == roleId && rm.Menu.MenuUrl == requestUrl && rm.Menu.IsDel == 1 )
                .Select(rm => rm.Menu)
                .ToListAsync();

            return roleMenus.Any();
        }
    }
}
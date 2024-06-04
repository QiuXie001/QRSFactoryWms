using DB.Models;
using IResponsitory.Sys;
using IServices.Sys;
using Microsoft.EntityFrameworkCore;
using IResponsitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Sys
{
    public class Sys_RoleMenuService : BaseService<SysRoleMenu>, ISys_RoleMenuService
    {
        private readonly ISys_RoleMenuResponsitory _responsitory;
        private readonly QrsfactoryWmsContext _dbContext;
        public Sys_RoleMenuService(
            QrsfactoryWmsContext dbContext,
            ISys_RoleMenuResponsitory responsitory
            ) : base(responsitory)
        {
            _responsitory = responsitory;
            _dbContext = dbContext;
        }

        public async Task DeleteByRoleIdAsync(long roleId)
        {
            // 使用DbContext的Set<T>方法获取SysRoleMenu表的DbSet
            var roleMenus = _dbContext.Set<SysRoleMenu>();

            // 查询出所有RoleId等于指定roleId的SysRoleMenu实体
            var roleMenuToDelete = roleMenus.Where(rm => rm.RoleId == roleId);

            // 从DbSet中移除这些实体
            roleMenus.RemoveRange(roleMenuToDelete);

            // 异步保存更改到数据库
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteByMenuIdAsync(long menuId)
        {
            // 使用DbContext的Set<T>方法获取SysRoleMenu表的DbSet
            var roleMenus = _dbContext.Set<SysRoleMenu>();

            // 查询出所有RoleId等于指定roleId的SysRoleMenu实体
            var roleMenuToDelete = roleMenus.Where(rm => rm.MenuId == menuId);

            // 从DbSet中移除这些实体
            roleMenus.RemoveRange(roleMenuToDelete);

            // 异步保存更改到数据库
            await _dbContext.SaveChangesAsync();
        }
    }
}

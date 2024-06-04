using SqlSugar;
using System.Collections.Generic;
using DB.Models;
using Qiu.Utils.Table;
using DB.Dto;

namespace IServices.Sys
{
    public interface ISys_RoleService : IBaseService<SysRole>
    {
        Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);

        Task<List<PermissionMenu>> GetMenuAsync();

        Task<List<PermissionMenu>> GetMenuAsync(long roleId, string menuType = "menu");

        Task<bool> InsertRole(SysRole role, long userId, string[] menuId);

        Task<bool> InsertMenu(SysMenu menu, long userId);

        Task<bool> UpdateRole(SysRole role, long userId, string[] menuId);

        Task<bool> UpdateMenu(SysMenu menu, long userId);

        Task<bool> DisableRole(SysRole role, long userId);

        Task<bool> DisableMenu(SysMenu menu, long userId);

        public Task<bool> DeleteRole(SysRole role);

        public Task<bool> DeleteMenu(SysMenu menu);

        Task<bool> CheckRoleAccessToUrl(long roleId, string requestUrl);

    }
}
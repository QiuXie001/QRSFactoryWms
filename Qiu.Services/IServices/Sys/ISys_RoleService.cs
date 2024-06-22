using DB.Dto;
using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Sys
{
    public interface ISys_RoleService : IBaseService<SysRole>
    {
        Task<Dictionary<long, string>> GetRoleNameList();
        Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);
        Task<string> GetRoleNameById(long roleId);
        Task<string> GetMenuAsync();
        Task<string> GetMenuAsync(Bootstrap.BootstrapParams bootstrap);

        Task<List<PermissionMenu>> GetMenuAsync(long roleId, string menuType = "menu");

        Task<bool> InsertRole(SysRole role, long userId, string[] menuId);

        Task<bool> InsertMenu(SysMenu menu, long userId);

        Task<bool> UpdateRole(RoleDto role, long userId, string[] menuId);

        Task<bool> UpdateMenu(SysMenu menu, long userId);

        Task<bool> DisableRole(SysRole role, long userId);

        Task<bool> DisableMenu(SysMenu menu, long userId);

        public Task<bool> DeleteRole(SysRole role);

        public Task<bool> DeleteMenu(SysMenu menu);

        Task<bool> CheckRoleAccessToUrl(long roleId, string requestUrl);

    }
}
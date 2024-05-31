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

        Task<bool> Insert(SysRole role, long userId, string[] menuId);

        Task<bool> Update(SysRole role, long userId, string[] menuId);
    }
}
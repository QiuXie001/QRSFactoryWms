using SqlSugar;
using System.Collections.Generic;
using DB.Models;
using Qiu.Utils.Table;
using Qiu.Core.Dto;

namespace IServices
{
    public interface ISys_RoleService : IBaseService<SysRole>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);

        List<PermissionMenu> GetMenu();

        List<PermissionMenu> GetMenu(long roleId, string menuType = "menu");

        Task<bool> Insert(SysRole role, long userId, string[] menuId);

        Task<bool> Update(SysRole role, long userId, string[] menuId);
    }
}
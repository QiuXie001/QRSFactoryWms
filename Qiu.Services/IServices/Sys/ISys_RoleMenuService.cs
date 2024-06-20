using DB.Models;

namespace IServices.Sys
{
    public interface ISys_RoleMenuService : IBaseService<SysRolemenu>
    {
        public Task DeleteByRoleIdAsync(long roleId);
        public Task DeleteByMenuIdAsync(long menuId);

    }
}

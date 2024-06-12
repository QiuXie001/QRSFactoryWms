using DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.Sys
{
    public interface ISys_RoleMenuService : IBaseService<SysRolemenu>
    {
        public Task DeleteByRoleIdAsync(long roleId);
        public Task DeleteByMenuIdAsync(long menuId);

    }
}

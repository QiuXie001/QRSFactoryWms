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
        private readonly ISys_RoleService _roleService;
        private readonly ISys_MenuService _menuService;
        private readonly QrsfactoryWmsContext _dbContext;
        public Sys_RoleMenuService(
            ISys_MenuService menuServices,
            ISys_RoleService roleService,
            QrsfactoryWmsContext dbContext,
            ISys_RoleMenuResponsitory responsitory
            ) : base(responsitory)
        {
            _responsitory = responsitory;
            _dbContext = dbContext;
            _roleService = roleService;
            _menuService = menuServices;
        }
    }
}

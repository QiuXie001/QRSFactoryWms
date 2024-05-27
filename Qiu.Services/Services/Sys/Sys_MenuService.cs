using DB.Models;
using IResponsitory.Sys;
using IServices.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Sys
{
    public class Sys_MenuService : BaseService<SysMenu>, ISys_MenuService
    {
        private readonly ISys_MenuResponsitory _responsitory;
        private readonly ISys_RoleService _roleService;
        private readonly ISys_RoleMenuService _rolemenuService;
        private readonly QrsfactoryWmsContext _dbContext;
        public Sys_MenuService(
            ISys_RoleMenuService rolemenuService,
            ISys_RoleService roleService,
            QrsfactoryWmsContext dbContext,
            ISys_MenuResponsitory responsitory
            ) : base(responsitory)
        {
            _responsitory = responsitory;
            _dbContext = dbContext;
            _roleService = roleService;
            _rolemenuService = rolemenuService;
        }

    }
}

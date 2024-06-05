using DB.Models;
using IRepository;
using IRepository.Sys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Sys
{
    public class Sys_RoleRepository : BaseRepository<SysRole>, ISys_RoleRepository
    {
        public Sys_RoleRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}

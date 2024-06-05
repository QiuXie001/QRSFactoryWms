using DB.Models;
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
    public class Sys_UserRespository : BaseRepository<SysUser>, ISys_UserRepository
    {
        public Sys_UserRespository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}

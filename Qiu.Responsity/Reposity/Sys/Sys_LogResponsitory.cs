using DB.Models;
using IRepository.Sys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Sys
{
    public class Sys_LogRepository : BaseRepository<SysLog>, ISys_LogRepository
    {
        private readonly QrsfactoryWmsContext _context;
        public Sys_LogRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}

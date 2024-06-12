using DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.Sys
{
    public interface ISys_SerialnumService : IBaseService<SysSerialnum>
    {
        public Task<string> GetSerialnumAsync(long userId, string tableName);

        public Task<SysSerialnum> GetSerialnumEntityAsync(long userId, string tableName);
    }
}

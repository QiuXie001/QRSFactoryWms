using DB.Models;
using Qiu.Utils.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.Sys
{
    public interface ISys_LogService : IBaseService<SysLog>
    {

        public Task<string> PageList(Bootstrap.BootstrapParams bootstrap);
        public Task<string> EChart(Bootstrap.BootstrapParams bootstrap);
    }
}

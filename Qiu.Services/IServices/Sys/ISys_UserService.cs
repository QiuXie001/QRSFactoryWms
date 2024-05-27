using DB.Models;
using Qiu.Utils.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.Sys
{
    public interface ISys_UserService : IBaseService<SysUser>
    {
        Task<(bool, string, SysUser)> CheckLoginAsync(SysUser dto);

        Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);
    }
}

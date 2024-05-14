using DB.Models;
using Qiu.Utils.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface ISys_UserService : IBaseService<SysUser>
    {
        Task<(bool, string, SysUser)> CheckLogin(SysUser dto);

        Task<string> PageList(Bootstrap.BootstrapParams bootstrap);
    }
}

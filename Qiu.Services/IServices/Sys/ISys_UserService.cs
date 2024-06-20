using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Sys
{
    public interface ISys_UserService : IBaseService<SysUser>
    {
        Task<(bool, string, SysUser)> CheckLoginAsync(SysUser dto);

        Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);

        Task<bool> Insert(SysUser user, long userId);

        Task<bool> Update(SysUser user, long userId);

        Task<bool> Disable(SysUser user, long userId);

        public Task<bool> Delete(SysUser user);

        Task<long> GetRoleAsync(long userId);

    }
}

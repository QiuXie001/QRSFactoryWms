using DB.Models;
using IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices
{
    public interface ISys_IdentityService : IBaseService<SysIdentity>
    {
        public Task<string> GenerateToken(long userId);
        public Task<bool> ValidateToken(string token, long userId);
        public Task<(bool, string)> RefreshToken(string token);
        public Task<bool> DeleteToken(string token);
        public Task<bool> DeleteToken(long userId);

    }
}

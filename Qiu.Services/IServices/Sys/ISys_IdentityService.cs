using DB.Models;

namespace IServices.Sys
{
    public interface ISys_IdentityService : IBaseService<SysIdentity>
    {
        public Task<string> GenerateToken(long userId);
        public Task<bool> ValidateToken(string token, long userId, string requestUrl);
        public Task<(bool, string)> RefreshToken(string token);
        public Task<bool> DeleteToken(string token);
        public Task<bool> DeleteToken(long userId);

    }
}

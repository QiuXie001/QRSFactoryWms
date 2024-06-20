using DB.Models;

namespace IServices.Sys
{
    public interface ISys_SerialnumService : IBaseService<SysSerialnum>
    {
        public Task<string> GetSerialnumAsync(long userId, string tableName);

        public Task<SysSerialnum> GetSerialnumEntityAsync(long userId, string tableName);
    }
}

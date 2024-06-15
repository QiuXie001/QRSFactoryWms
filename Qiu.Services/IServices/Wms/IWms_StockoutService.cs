using DB.Models;
using Qiu.Core.Dto;

namespace IServices.Wms
{
    public interface IWms_StockoutService : IBaseService<WmsStockout>
    {
        Task<string> PageListAsync(PubParams.StockOutBootstrapParams bootstrap);

        string PrintList(long stockInId);

        Task<bool> AuditinAsync(long userId, long stockOutId);
    }
}
using DB.Models;
using Qiu.Core.Dto;

namespace IServices.Wms
{
    public interface IWms_StockinService : IBaseService<WmsStockin>
    {
        Task<string> PageListAsync(PubParams.StockInBootstrapParams bootstrap);

        string PrintList(long stockInId);

        Task<bool> AuditinAsync(long UserId, long stockInId);
    }
}
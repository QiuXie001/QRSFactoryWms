using DB.Models;
using Qiu.Core.Dto;

namespace IServices.Wms
{
    public interface IWms_StockinService : IBaseService<WmsStockin>
    {
        Task<string> PageListAsync(PubParams.StockInBootstrapParams bootstrap);

        Task<string> PrintListAsync(string stockInId);

        Task<bool> Auditin(long UserId, long stockInId);
    }
}
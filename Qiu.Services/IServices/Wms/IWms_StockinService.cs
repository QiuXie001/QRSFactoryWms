using DB.Models;
using Qiu.Core.Dto;

namespace IServices.Wms
{
    public interface IWms_StockinService : IBaseService<WmsStockin>
    {
        Task<string> PageList(PubParams.StockInBootstrapParams bootstrap);

        Task<string> PrintList(string stockInId);

        Task<bool> Auditin(long UserId, long stockInId);
    }
}
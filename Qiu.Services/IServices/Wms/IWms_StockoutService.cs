using DB.Models;
using Qiu.Core.Dto;

namespace IServices.Wms
{
    public interface IWms_StockoutService : IBaseService<WmsStockout>
    {
        Task<string> PageList(PubParams.StockOutBootstrapParams bootstrap);

        Task<string> PrintList(string stockInId);

        Task<bool> Auditin(long userId, long stockOutId);
    }
}
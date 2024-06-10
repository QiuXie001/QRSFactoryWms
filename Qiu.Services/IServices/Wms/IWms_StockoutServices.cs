using DB.Models;
using Qiu.Core.Dto;

namespace IServices
{
    public interface IWms_StockoutServices : IBaseService<WmsStockout>
    {
        string PageList(PubParams.StockOutBootstrapParams bootstrap);

        string PrintList(string stockInId);

        bool Auditin(long userId, long stockOutId);
    }
}
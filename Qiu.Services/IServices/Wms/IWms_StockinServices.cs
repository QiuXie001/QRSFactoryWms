using DB.Models;
using Qiu.Core.Dto;

namespace IServices
{
    public interface IWms_StockinServices : IBaseService<WmsStockin>
    {
        string PageList(PubParams.StockInBootstrapParams bootstrap);

        string PrintList(string stockInId);

        bool Auditin(long UserId, long stockInId);
    }
}
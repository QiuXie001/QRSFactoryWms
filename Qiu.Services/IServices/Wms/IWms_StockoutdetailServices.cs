using DB.Models;

namespace IServices
{
    public interface IWms_StockoutdetailServices : IBaseService<WmsStockoutdetail>
    {
        string PageList(string pid);
    }
}
using DB.Models;

namespace IServices.Wms
{
    public interface IWms_StockoutdetailService : IBaseService<WmsStockoutdetail>
    {
        Task<string> PageList(string pid);
    }
}
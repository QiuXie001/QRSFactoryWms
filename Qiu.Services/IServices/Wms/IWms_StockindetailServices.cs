using DB.Models;

namespace IServices
{
    public interface IWms_StockindetailServices : IBaseService<WmsStockindetail>
    {
        string PageList(string pid);
    }
}
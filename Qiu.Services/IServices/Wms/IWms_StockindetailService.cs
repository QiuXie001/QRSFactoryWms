using DB.Models;

namespace IServices.Wms
{
    public interface IWms_StockindetailService : IBaseService<WmsStockindetail>
    {
        Task<string> PageListAsync(string pid);
    }
}
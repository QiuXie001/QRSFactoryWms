using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Wms
{
    public interface IWms_SupplierService : IBaseService<WmsSupplier>
    {
        Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);
    }
}
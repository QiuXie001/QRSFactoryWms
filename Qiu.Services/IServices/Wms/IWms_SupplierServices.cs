using DB.Models;
using Qiu.Utils.Table;

namespace IServices
{
    public interface IWms_SupplierServices : IBaseService<WmsSupplier>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);
    }
}
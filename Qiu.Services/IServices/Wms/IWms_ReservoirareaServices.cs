using DB.Models;
using Qiu.Utils.Table;

namespace IServices
{
    public interface IWms_ReservoirareaServices : IBaseService<WmsReservoirarea>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);
    }
}
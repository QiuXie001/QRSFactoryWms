using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Wms
{
    public interface IWms_ReservoirareaService : IBaseService<WmsReservoirarea>
    {
        Task<string> PageList(Bootstrap.BootstrapParams bootstrap);
    }
}
using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Wms
{
    public interface IWms_MaterialService : IBaseService<WmsMaterial>
    {
        Task<string> PageList(Bootstrap.BootstrapParams bootstrap);

        Task<byte[]> ExportList(Bootstrap.BootstrapParams bootstrap);
    }
}
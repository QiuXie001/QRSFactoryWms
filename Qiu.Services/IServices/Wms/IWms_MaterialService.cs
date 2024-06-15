using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Wms
{
    public interface IWms_MaterialService : IBaseService<WmsMaterial>
    {
        Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);

        Task<byte[]> ExportListAsync(Bootstrap.BootstrapParams bootstrap);
    }
}
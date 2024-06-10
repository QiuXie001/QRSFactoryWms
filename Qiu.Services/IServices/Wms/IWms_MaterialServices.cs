using DB.Models;
using Qiu.Utils.Table;

namespace IServices
{
    public interface IWms_MaterialServices : IBaseService<WmsMaterial>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);

        byte[] ExportList(Bootstrap.BootstrapParams bootstrap);
    }
}
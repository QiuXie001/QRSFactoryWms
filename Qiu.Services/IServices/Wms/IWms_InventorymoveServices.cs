using DB.Models;
using Qiu.Utils.Table;

namespace IServices
{
    public interface IWms_InventorymoveServices : IBaseService<WmsInventorymove>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);

        bool Auditin(long userId, long InventorymoveId);

        string PrintList(string InventorymoveId);
    }
}
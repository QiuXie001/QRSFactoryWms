using System.Data;
using DB.Models;
using Qiu.Utils.Table;

namespace IServices
{
    public interface IWms_CustomerServices : IBaseService<WmsCustomer>
    {
        string PageList(Bootstrap.BootstrapParams bootstrap);

        (bool, string) Import(DataTable dt, long userId);
    }
}
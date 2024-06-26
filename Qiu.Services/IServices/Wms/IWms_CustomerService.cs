using DB.Models;
using Qiu.Utils.Table;
using System.Data;

namespace IServices.Wms
{
    public interface IWms_CustomerService : IBaseService<WmsCustomer>
    {
        Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);

        Task<Dictionary<long, string>> GetCustomerList();
        Task<(bool, string)> ImportAsync(DataTable dt, long userId);
    }
}
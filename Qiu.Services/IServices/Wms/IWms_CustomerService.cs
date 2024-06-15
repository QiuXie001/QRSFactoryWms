using System.Data;
using DB.Models;
using Qiu.Utils.Table;

namespace IServices.Wms
{
    public interface IWms_CustomerService : IBaseService<WmsCustomer>
    {
        Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap);

        Task<(bool, string)> Import(DataTable dt, long userId);
    }
}
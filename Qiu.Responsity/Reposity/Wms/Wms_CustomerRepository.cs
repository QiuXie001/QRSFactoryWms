using IRepository;
using DB.Models;

namespace Repository
{
    public class Wms_CustomerRepository : BaseRepository<WmsCustomer>, IWms_CustomerRepository
    {
        public Wms_CustomerRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
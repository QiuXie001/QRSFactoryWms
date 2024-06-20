using DB.Models;
using IRepository.Wms;

namespace Repository
{
    public class Wms_CustomerRepository : BaseRepository<WmsCustomer>, IWms_CustomerRepository
    {
        public Wms_CustomerRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
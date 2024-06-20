using DB.Models;
using IRepository.Wms;

namespace Repository
{
    public class Wms_SupplierRepository : BaseRepository<WmsSupplier>, IWms_SupplierRepository
    {
        public Wms_SupplierRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
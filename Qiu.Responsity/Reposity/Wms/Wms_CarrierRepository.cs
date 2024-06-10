using IRepository;
using DB.Models;

namespace Repository
{
    public class Wms_CarrierRepository : BaseRepository<WmsCarrier>, IWms_CarrierRepository
    {
        public Wms_CarrierRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
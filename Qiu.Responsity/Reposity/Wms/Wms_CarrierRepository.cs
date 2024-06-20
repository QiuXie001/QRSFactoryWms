using DB.Models;
using IRepository.Wms;

namespace Repository
{
    public class Wms_CarrierRepository : BaseRepository<WmsCarrier>, IWms_CarrierRepository
    {
        public Wms_CarrierRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
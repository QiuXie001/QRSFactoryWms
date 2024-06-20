using DB.Models;
using IRepository.Wms;

namespace Repository
{
    public class Wms_DeliveryRepository : BaseRepository<WmsDelivery>, IWms_DeliveryRepository
    {
        public Wms_DeliveryRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
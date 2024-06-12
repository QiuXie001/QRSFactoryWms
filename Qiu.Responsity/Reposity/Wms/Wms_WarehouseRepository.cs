using IRepository;
using DB.Models;
using IRepository.Wms;

namespace Repository
{
    public class Wms_WarehouseRepository : BaseRepository<WmsWarehouse>, IWms_WarehouseRepository
    {
        public Wms_WarehouseRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
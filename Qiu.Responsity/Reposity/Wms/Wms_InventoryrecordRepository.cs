using DB.Models;
using IRepository.Wms;

namespace Repository
{
    public class Wms_InventoryrecordRepository : BaseRepository<WmsInventoryrecord>, IWms_InventoryrecordRepository
    {
        public Wms_InventoryrecordRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
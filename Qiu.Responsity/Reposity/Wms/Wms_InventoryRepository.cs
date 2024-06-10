using IRepository;
using DB.Models;

namespace Repository
{
    public class Wms_InventoryRepository : BaseRepository<WmsInventory>, IWms_InventoryRepository
    {
        public Wms_InventoryRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
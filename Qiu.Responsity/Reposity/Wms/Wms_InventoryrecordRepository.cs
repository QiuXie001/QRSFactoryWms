using IRepository;
using DB.Models;

namespace Repository
{
    public class Wms_InventoryrecordRepository : BaseRepository<WmsInventoryrecord>, IWms_InventoryrecordRepository
    {
        public Wms_InventoryrecordRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
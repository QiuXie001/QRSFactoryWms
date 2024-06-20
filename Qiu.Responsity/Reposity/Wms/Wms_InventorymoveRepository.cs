using DB.Models;
using IRepository.Wms;

namespace Repository
{
    public class Wms_InventorymoveRepository : BaseRepository<WmsInventorymove>, IWms_InventorymoveRepository
    {
        public Wms_InventorymoveRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
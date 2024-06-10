using IRepository;
using DB.Models;

namespace Repository
{
    public class Wms_InventorymoveRepository : BaseRepository<WmsInventorymove>, IWms_InventorymoveRepository
    {
        public Wms_InventorymoveRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
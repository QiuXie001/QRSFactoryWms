using IRepository;
using DB.Models;

namespace Repository
{
    public class Wms_MaterialRepository : BaseRepository<WmsMaterial>, IWms_MaterialRepository
    {
        public Wms_MaterialRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
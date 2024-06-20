using DB.Models;
using IRepository.Wms;

namespace Repository
{
    public class Wms_MaterialRepository : BaseRepository<WmsMaterial>, IWms_MaterialRepository
    {
        public Wms_MaterialRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
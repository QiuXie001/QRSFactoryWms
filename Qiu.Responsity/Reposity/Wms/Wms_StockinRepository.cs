using IRepository;
using DB.Models;

namespace Repository
{
    public class Wms_StockinRepository : BaseRepository<WmsStockin>, IWms_StockinRepository
    {
        public Wms_StockinRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
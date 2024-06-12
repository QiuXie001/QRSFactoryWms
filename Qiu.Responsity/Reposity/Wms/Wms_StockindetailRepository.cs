using IRepository;
using DB.Models;
using IRepository.Wms;

namespace Repository
{
    public class Wms_StockindetailRepository : BaseRepository<WmsStockindetail>, IWms_StockindetailRepository
    {
        public Wms_StockindetailRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
using IRepository;
using DB.Models;

namespace Repository
{
    public class Wms_StockoutdetailRepository : BaseRepository<WmsStockoutdetail>, IWms_StockoutdetailRepository
    {
        public Wms_StockoutdetailRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
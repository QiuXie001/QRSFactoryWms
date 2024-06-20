using DB.Models;
using IRepository.Wms;

namespace Repository
{
    public class Wms_StockoutdetailRepository : BaseRepository<WmsStockoutdetail>, IWms_StockoutdetailRepository
    {
        public Wms_StockoutdetailRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
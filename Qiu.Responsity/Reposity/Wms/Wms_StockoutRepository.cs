using IRepository;
using DB.Models;
using IRepository.Wms;

namespace Repository
{
    public class Wms_StockoutRepository : BaseRepository<WmsStockout>, IWms_StockoutRepository
    {
        public Wms_StockoutRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
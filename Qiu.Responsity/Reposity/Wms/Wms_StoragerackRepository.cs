using IRepository;
using DB.Models;

namespace Repository
{
    public class Wms_StoragerackRepository : BaseRepository<WmsStoragerack>, IWms_StoragerackRepository
    {
        public Wms_StoragerackRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
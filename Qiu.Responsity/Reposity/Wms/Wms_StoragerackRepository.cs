using DB.Models;
using IRepository.Wms;

namespace Repository
{
    public class Wms_StoragerackRepository : BaseRepository<WmsStoragerack>, IWms_StoragerackRepository
    {
        public Wms_StoragerackRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
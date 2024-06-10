using IRepository;
using DB.Models;

namespace Repository
{
    public class Wms_ReservoirareaRepository : BaseRepository<WmsReservoirarea>, IWms_ReservoirareaRepository
    {
        public Wms_ReservoirareaRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
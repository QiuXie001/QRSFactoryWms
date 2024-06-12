using IRepository;
using DB.Models;
using IRepository.Wms;

namespace Repository
{
    public class Wms_ReservoirareaRepository : BaseRepository<WmsReservoirarea>, IWms_ReservoirareaRepository
    {
        public Wms_ReservoirareaRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
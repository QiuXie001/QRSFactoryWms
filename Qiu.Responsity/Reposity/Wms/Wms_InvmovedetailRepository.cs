using DB.Models;
using IRepository.Wms;

namespace Repository
{
    public class Wms_InvmovedetailRepository : BaseRepository<WmsInvmovedetail>, IWms_InvmovedetailRepository
    {
        public Wms_InvmovedetailRepository(QrsfactoryWmsContext dbContext) : base(dbContext)
        {
        }
    }
}
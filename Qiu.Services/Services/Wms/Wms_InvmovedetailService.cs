using IRepository;
using IServices;
using DB.Models;
using Qiu.Utils.Json;
using Qiu.Utils.Table;
using IServices.Wms;
using IRepository.Wms;

namespace Services
{
    public class Wms_InvmovedetailService : BaseService<WmsInvmovedetail>, IWms_InvmovedetailService
    {
        private readonly IWms_InvmovedetailRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_InvmovedetailService(IWms_InvmovedetailRepository repository
            ,
            QrsfactoryWmsContext dbContext) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public Task<string> PageList(string pid)
        {
            throw new NotImplementedException();
        }
    }
}
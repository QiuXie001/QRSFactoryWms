using IRepository;
using IServices;
using DB.Models;
using Qiu.Utils.Json;
using Qiu.Utils.Table;
using IRepository.Wms;
using IServices.Wms;

namespace Services
{
    public class Wms_StockindetailService : BaseService<WmsStockindetail>, IWms_StockindetailService
    {
        private readonly IWms_StockindetailRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_StockindetailService(QrsfactoryWmsContext dbContext, IWms_StockindetailRepository repository) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public Task<string> PageList(string pid)
        {
            throw new NotImplementedException();
        }
    }
}
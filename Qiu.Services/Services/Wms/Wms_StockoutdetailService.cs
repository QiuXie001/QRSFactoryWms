using IRepository;
using IServices;
using DB.Models;
using Qiu.Utils.Json;
using Qiu.Utils.Table;
using IRepository.Wms;
using IServices.Wms;

namespace Services
{
    public class Wms_StockoutdetailService : BaseService<WmsStockoutdetail>, IWms_StockoutdetailService
    {
        private readonly IWms_StockoutdetailRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_StockoutdetailService(IWms_StockoutdetailRepository repository,
            QrsfactoryWmsContext dbContext
            ) : base(repository)
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
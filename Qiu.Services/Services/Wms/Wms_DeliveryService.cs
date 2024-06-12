using IRepository;
using IServices;
using DB.Models;
using System;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IServices.Wms;
using IRepository.Wms;

namespace Services
{
    public class Wms_DeliveryService : BaseService<WmsDelivery>, IWms_DeliveryService
    {
        private readonly IWms_DeliveryRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_DeliveryService(IWms_DeliveryRepository repository,
            QrsfactoryWmsContext dbContext
            ) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public Task<bool> Delivery(WmsDelivery delivery)
        {
            throw new NotImplementedException();
        }

        public Task<string> PageList(Bootstrap.BootstrapParams bootstrap)
        {
            throw new NotImplementedException();
        }
    }
}
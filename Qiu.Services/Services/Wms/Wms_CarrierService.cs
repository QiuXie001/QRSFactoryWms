using IRepository;
using IServices;
using Qiu.Utils.Table;
using DB.Models;
using System;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Table;
using IServices.Wms;
using IRepository.Wms;

namespace Services
{
    public class Wms_CarrierService : BaseService<WmsCarrier>, IWms_CarrierService
    {
        private readonly IWms_CarrierRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_CarrierService(QrsfactoryWmsContext dbContext, IWms_CarrierRepository repository) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public Task<string> PageList(Bootstrap.BootstrapParams bootstrap)
        {
            throw new NotImplementedException();
        }
    }
}
using IRepository;
using IServices;
using DB.Models;
using System;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Table;
using IRepository.Wms;
using IServices.Wms;

namespace Services
{
    public class Wms_StoragerackService : BaseService<WmsStoragerack>, IWms_StoragerackService
    {
        private readonly IWms_StoragerackRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_StoragerackService(
            QrsfactoryWmsContext dbContext,
            IWms_StoragerackRepository repository) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public Task<string> PageList(Bootstrap.BootstrapParams bootstrap)
        {
            throw new NotImplementedException();
        }
    }
}
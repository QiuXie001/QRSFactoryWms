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
    public class Wms_ReservoirareaService : BaseService<WmsReservoirarea>, IWms_ReservoirareaService
    {
        private readonly IWms_ReservoirareaRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_ReservoirareaService(QrsfactoryWmsContext dbContext, IWms_ReservoirareaRepository repository) : base(repository)
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
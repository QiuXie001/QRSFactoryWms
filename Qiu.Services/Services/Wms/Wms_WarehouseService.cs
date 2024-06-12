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
    public class Wms_WarehouseService : BaseService<WmsWarehouse>, IWms_WarehouseService
    {
        private readonly IWms_WarehouseRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_WarehouseService(QrsfactoryWmsContext dbContext, IWms_WarehouseRepository repository) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public Task<string> PageList(Bootstrap.BootstrapParams bootstrap)
        {
           
        }
    }
}
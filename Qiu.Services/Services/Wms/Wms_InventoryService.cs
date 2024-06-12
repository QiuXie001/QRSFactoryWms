using IRepository;
using IServices;
using DB.Models;
using System;
using Qiu.Core.Dto;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Table;
using IServices.Wms;
using IRepository.Wms;

namespace Services
{
    public class Wms_InventoryService : BaseService<WmsInventory>, IWms_InventoryService
    {
        private readonly IWms_InventoryRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_InventoryService(
            QrsfactoryWmsContext dbContext,
            IWms_InventoryRepository repository) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public Task<string> PageList(PubParams.InventoryBootstrapParams bootstrap)
        {
            throw new NotImplementedException();
        }

        public Task<string> SearchInventory(PubParams.InventoryBootstrapParams bootstrap)
        {
            throw new NotImplementedException();
        }
    }
}
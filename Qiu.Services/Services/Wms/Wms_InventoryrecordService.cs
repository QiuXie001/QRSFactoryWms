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
    public class Wms_InventoryrecordService : BaseService<WmsInventoryrecord>, IWms_InventoryrecordService
    {
        private readonly IWms_InventoryrecordRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_InventoryrecordService(
            QrsfactoryWmsContext dbContext,
            IWms_InventoryrecordRepository repository) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public Task<string> PageList(PubParams.InventoryBootstrapParams bootstrap)
        {
            throw new NotImplementedException();
        }
    }
}
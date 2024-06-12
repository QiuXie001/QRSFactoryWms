using IRepository;
using IServices;
using Microsoft.AspNetCore.Hosting;
using DB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Qiu.Core.Dto;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IServices.Wms;
using IRepository.Wms;

namespace Services
{
    public class Wms_StockinService : BaseService<WmsStockin>, IWms_StockinService
    {
        private readonly IWms_StockinRepository _repository;
        private readonly IWms_StockindetailRepository _detail;
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly IWms_InventoryRepository _inventory;
        private readonly IWms_InventoryrecordRepository _inventoryrecord;
        private readonly IWebHostEnvironment _env;

        public Wms_StockinService(
            QrsfactoryWmsContext dbContext,
            IWms_InventoryRepository inventoryRepository,
            IWms_InventoryrecordRepository inventoryrecordRepository,
            IWms_StockindetailRepository detail,
            IWebHostEnvironment env,
            IWms_StockinRepository repository) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
            _detail = detail;
            _inventory = inventoryRepository;
            _inventoryrecord = inventoryrecordRepository;
            _env = env;
        }

        public Task<bool> Auditin(long UserId, long stockInId)
        {
            throw new NotImplementedException();
        }

        public Task<string> PageList(PubParams.StockInBootstrapParams bootstrap)
        {
            throw new NotImplementedException();
        }

        public Task<string> PrintList(string stockInId)
        {
            throw new NotImplementedException();
        }
    }
}
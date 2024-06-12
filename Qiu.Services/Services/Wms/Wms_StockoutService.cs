using IRepository;
using IServices;
using Microsoft.AspNetCore.Hosting;
using DB.Models;
using System;
using System.IO;
using System.Linq;
using Qiu.Core.Dto;
using Qiu.Utils.Check;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IRepository.Wms;
using IServices.Wms;

namespace Services
{
    public class Wms_StockoutService : BaseService<WmsStockout>, IWms_StockoutService
    {
        private readonly IWms_StockoutRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly IWms_StockoutdetailRepository _detail;
        private readonly IWms_InventoryRepository _inventory;

        public Wms_StockoutService(QrsfactoryWmsContext dbContext,
            IWms_StockoutRepository repository,
            IWebHostEnvironment env,
            IWms_StockoutdetailRepository detail,
            IWms_InventoryRepository inventory
            ) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
            _env = env;
            _detail = detail;
            _inventory = inventory;
        }

        public Task<bool> Auditin(long userId, long stockOutId)
        {
            throw new NotImplementedException();
        }

        public Task<string> PageList(PubParams.StockOutBootstrapParams bootstrap)
        {
            throw new NotImplementedException();
        }

        public Task<string> PrintList(string stockInId)
        {
            throw new NotImplementedException();
        }
    }
}
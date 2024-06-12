using IRepository;
using IServices;
using Microsoft.AspNetCore.Hosting;
using DB.Models;
using System;
using System.IO;
using System.Linq;
using Qiu.Utils.Check;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IRepository.Wms;
using IServices.Wms;

namespace Services
{
    public class Wms_InventorymoveService : BaseService<WmsInventorymove>, IWms_InventorymoveService
    {
        private readonly IWms_InventorymoveRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public Wms_InventorymoveService(IWms_InventorymoveRepository repository,
            QrsfactoryWmsContext dbContext,
            IWebHostEnvironment env
            ) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
            _env = env;
        }

        public Task<bool> Auditin(long userId, long InventorymoveId)
        {
            throw new NotImplementedException();
        }

        public Task<string> PageList(Bootstrap.BootstrapParams bootstrap)
        {
            throw new NotImplementedException();
        }

        public Task<string> PrintList(string InventorymoveId)
        {
            throw new NotImplementedException();
        }
    }
}
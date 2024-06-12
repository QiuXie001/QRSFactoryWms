using IRepository;
using IServices;
using DB.Models;
using System;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IRepository.Wms;
using IServices.Wms;

namespace Services
{
    public class Wms_MaterialService : BaseService<WmsMaterial>, IWms_MaterialService
    {
        private readonly IWms_MaterialRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_MaterialService(QrsfactoryWmsContext dbContext, IWms_MaterialRepository repository) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public Task<byte[]> ExportList(Bootstrap.BootstrapParams bootstrap)
        {
            throw new NotImplementedException();
        }

        public Task<string> PageList(Bootstrap.BootstrapParams bootstrap)
        {
            throw new NotImplementedException();
        }
    }
}
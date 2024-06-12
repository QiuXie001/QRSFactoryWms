using IRepository;
using IServices;
using DB.Models;
using System;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Table;
using IServices.Wms;
using IRepository.Wms;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class Wms_SupplierService : BaseService<WmsSupplier>, IWms_SupplierService
    {
        private readonly IWms_SupplierRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_SupplierService(QrsfactoryWmsContext dbContext, IWms_SupplierRepository repository) : base(repository)
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
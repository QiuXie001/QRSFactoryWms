using IRepository;
using IServices;
using DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Log;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IServices.Wms;
using IRepository.Wms;
using System.Data;

namespace Services
{
    public class Wms_CustomerService : BaseService<WmsCustomer>, IWms_CustomerService
    {
        private readonly IWms_CustomerRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_CustomerService(QrsfactoryWmsContext dbContext, IWms_CustomerRepository repository) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public Task<(bool, string)> Import(DataTable dt, long userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> PageList(Bootstrap.BootstrapParams bootstrap)
        {
            throw new NotImplementedException();
        }
    }
}
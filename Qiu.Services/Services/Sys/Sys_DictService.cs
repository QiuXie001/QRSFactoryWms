using DB.Models;
using IRepository.Sys;
using IServices.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Sys
{
    public class Sys_DictService : BaseService<SysDict>, ISys_DictService
    {
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly ISys_DictRepository _repository;
        public Sys_DictService(
            QrsfactoryWmsContext dbContext,
            ISys_DictRepository repository
            ) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }
    }
}

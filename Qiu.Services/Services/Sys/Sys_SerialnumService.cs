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
    public class Sys_SerialnumService : BaseService<SysSerialNum>, ISys_SerialnumService
    {
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly ISys_SerialnumRepository _repository;
        public Sys_SerialnumService(
            QrsfactoryWmsContext dbContext,
            ISys_SerialnumRepository repository
            ) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

    }
}

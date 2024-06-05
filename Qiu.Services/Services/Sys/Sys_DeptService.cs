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
    public class Sys_DeptService : BaseService<SysDept>, ISys_DeptService
    {
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly ISys_DeptRepository _repository;
        private readonly ISys_UserService _userService;
        public Sys_DeptService(
            QrsfactoryWmsContext dbContext,
            ISys_DeptRepository repository,
            ISys_UserService userService
            ) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
            _userService = userService;
        }
    }
}

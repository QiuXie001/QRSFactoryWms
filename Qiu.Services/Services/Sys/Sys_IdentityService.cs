using DB.Models;
using EntityFrameworkCore.RepositoryInfrastructure;
using IRepository.Sys;
using IServices.Sys;
using Qiu.Utils.Env;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Sys
{
    public class Sys_IdentityService : BaseService<SysIdentity>, ISys_IdentityService
    {
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly ISys_IdentityRepository _repository;
        private readonly ISys_RoleService _roleService;
        private readonly ISys_UserService _userService;
        public Sys_IdentityService(
            QrsfactoryWmsContext dbContext,
            ISys_IdentityRepository repository,
            ISys_RoleService roleService,
            ISys_UserService userService
            ) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
            _roleService = roleService;
            _userService = userService;
        }

        //生成
        public async Task<string> GenerateToken(long userId)
        {
            var token = Guid.NewGuid().ToString();
            //禁用所有token，保证仅有一个token生效
            await DeleteToken(userId);
            await _repository.InsertAsync(new SysIdentity
            {
                Token = token,
                GeneratedTime = DateTime.Now,
                ExpirationTime = DateTime.Now.AddDays(1),
                UserId = userId,
                IsEabled = 1,
                LoginIp = GlobalCore.GetIp()
            });
            return token;
        }

        //验证
        public async Task<bool> ValidateToken(string token, long userId, string requestUrl)
        {
            var identity = await _repository.QueryableToSingleAsync(
                p => p.Token == token
                && p.UserId == userId
                && p.LoginIp == GlobalCore.GetIp()
                && p.IsEabled == 1);

            if (identity == null)
            {
                return false;
            }

            if (DateTime.UtcNow > identity.ExpirationTime)
            {
                return false;
            }

            // 获取用户的角色ID
            var roleId = await _userService.GetRoleAsync(userId);

            // 检查角色是否有权限访问请求的URL
            if (!await _roleService.CheckRoleAccessToUrl(roleId, requestUrl))
            {
                return false;
            }

            return true;
        }

        //刷新
        public async Task<(bool, string)> RefreshToken(string token)
        {
            var flag = true;
            var identity = await _repository.QueryableToSingleAsync(p => p.Token == token && p.IsEabled == 1);
            if (identity == null)
            {
                flag = false;
                return (flag, "");
            }
            identity.ExpirationTime = DateTime.UtcNow.AddDays(1);
            await _repository.UpdateAsync(identity);
            return (flag, identity.Token);
        }

        //禁用token
        public async Task<bool> DeleteToken(string token)
        {
            var flag = true;
            var identity = await _repository.QueryableToSingleAsync(p => p.Token == token && p.IsEabled == 1);
            if (identity == null)
            {
                flag = false;
                return flag;
            }
            identity.IsEabled = 0;//禁用
            await _repository.UpdateAsync(identity);
            return flag;
        }

        //禁用user
        public async Task<bool> DeleteToken(long userId)
        {
            var flag = true;
            var identity = await _repository.QueryableToListAsync(p => p.UserId == userId && p.IsEabled == 1);
            if (identity == null)
            {
                flag = false;
                return flag;
            }
            foreach (var item in identity)
            {
                item.IsEabled = 0;//禁用
                await _repository.UpdateAsync(item);
            }
            return flag;
        }


    }
}

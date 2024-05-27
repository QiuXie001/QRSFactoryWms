using DB.Models;
using EntityFrameworkCore.RepositoryInfrastructure;
using IResponsitory.Sys;
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
        private readonly ISys_IdentityResponsitory _repository;
        public Sys_IdentityService(
            QrsfactoryWmsContext dbContext,
            ISys_IdentityResponsitory repository
            ) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
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
        public async Task<bool> ValidateToken(string token, long userId)
        {
            var flag = true;
            var identity = await _repository.QueryableToSingleAsync(
                p => p.Token == token
                && p.UserId == userId
                && p.LoginIp == GlobalCore.GetIp()
                && p.IsEabled == 1);
            if (identity == null)
            {
                flag = false;
            }
            else if (DateTime.UtcNow > identity.ExpirationTime)
            {
                flag = false;
            }
            return flag;
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

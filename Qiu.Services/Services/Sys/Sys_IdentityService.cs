using DB.Models;
using EntityFrameworkCore.RepositoryInfrastructure;
using IRepository.Sys;
using IServices.Sys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Qiu.Utils.Env;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        private readonly IServiceProvider _serviceProvider;
        public Sys_IdentityService(
            QrsfactoryWmsContext dbContext,
            ISys_IdentityRepository repository,
            ISys_RoleService roleService,
            ISys_UserService userService,
            IServiceProvider serviceProvider
            ) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
            _roleService = roleService;
            _userService = userService;
            _serviceProvider = serviceProvider;
        }

        //生成
        public async Task<string> GenerateToken(long userId)
        {
            var configuration = _serviceProvider.GetService<IConfiguration>();
            var secretKey = configuration["Jwt:SecretKey"];

            var existingToken = await _repository.QueryableToSingleAsync(x => x.UserId == userId && x.IsEabled == 1);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = configuration["Jwt:Issuer"],
            };

            var tokenString = existingToken?.Token;
            if (existingToken == null || existingToken.ExpirationTime < DateTime.UtcNow)
            {
                var token = tokenHandler.CreateToken(tokenDescriptor);
                tokenString = tokenHandler.WriteToken(token);

                if (existingToken != null)
                {
                    await RefreshToken(existingToken.Token);
                }
                else
                {
                    // 创建新的令牌记录
                    await _repository.InsertAsync(new SysIdentity
                    {
                        Token = tokenString,
                        GeneratedTime = DateTime.Now,
                        ExpirationTime = DateTime.Now.AddDays(7),
                        UserId = userId,
                        IsEabled = 1,
                        LoginIp = GlobalCore.GetIp()
                    });
                }
            }
            else
            {
                // 如果现有令牌仍然有效，刷新其过期时间
                existingToken.ExpirationTime = DateTime.UtcNow.AddDays(7);
                await _repository.UpdateAsync(existingToken);
            }

            return tokenString;
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
            identity.ExpirationTime = DateTime.UtcNow.AddDays(7);
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

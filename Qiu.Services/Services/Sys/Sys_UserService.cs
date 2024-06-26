﻿using DB.Models;
using IRepository.Sys;
using IServices.Sys;
using Microsoft.EntityFrameworkCore;
using Qiu.Utils.Extensions;
using Qiu.Utils.Pub;
using Qiu.Utils.Security;
using Qiu.Utils.Table;
using System.Linq.Expressions;
using System.Text.Json;

namespace Services.Sys
{
    public class Sys_UserService : BaseService<SysUser>, ISys_UserService
    {
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly ISys_UserRepository _repository;
        public Sys_UserService(
            QrsfactoryWmsContext dbContext,
            ISys_UserRepository repository
            ) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;

        }

        public async Task<(bool, string, SysUser?)> CheckLoginAsync(SysUser dto)
        {
            var flag = true;
            if (dto.IsNullT())
            {
                flag = false;
                return (flag, PubConst.Login2, null);
            }

            Console.WriteLine(dto.UserName);

            var sys = await _repository.QueryableToSingleAsync(c => c.UserName == dto.UserName && c.IsDel == 1);

            if (sys.IsNullT())
            {
                flag = false;
                return (flag, PubConst.Login4, null);
            }
            if (sys.IsEabled == 0)
            {
                flag = false;
                return (flag, PubConst.Login3, null);
            }
            if (sys.Pwd != dto.Pwd.ToMd5())
            {
                flag = false;
                return (flag, PubConst.Login2, null);
            }
            else
            {
                sys.LoginDate = DateTime.Now.ToString("D").ToDateTime();
                sys.LoginTime = DateTime.Now.ToString("HHmmss").ToInt32();
                await _repository.UpdateAsync(sys);
                return (flag, PubConst.Login1, new SysUser
                {
                    UserId = sys.UserId,
                    UserName = sys.UserName,
                    UserNickname = sys.UserNickname,
                    RoleId = sys.RoleId,
                    HeadImg = sys.HeadImg
                });
            }
        }

        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            var totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<SysUser>()
                .Include(s => s.CreateByUser)
                .Include(s => s.ModifiedByUser)
                .Include(s => s.Dept)
                .Include(s => s.Role)
                .Where(s => s.IsDel == 1 && s.Dept.IsDel == 1 && s.Role.IsDel == 1)
                .Select(s => new
                {
                    UserId = s.UserId.ToString(),
                    s.UserName,
                    s.UserNickname,
                    s.Dept.DeptId,
                    s.Dept.DeptName,
                    s.Role.RoleId,
                    s.Role.RoleName,
                    s.Tel,
                    s.Email,
                    s.Sex,
                    s.IsEabled,
                    s.Remark,
                    CName = s.CreateByUser.UserNickname,
                    s.CreateDate,
                    UName = s.ModifiedByUser.UserNickname,
                    s.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(s => s.UserName.Contains(bootstrap.search) || s.UserNickname.Contains(bootstrap.search));
            }

            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query = query.Where(s => s.CreateDate > bootstrap.datemin.ToDateTimeB() && s.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }

            var list = await query.Skip((pageNumber - 1) * bootstrap.limit)
                                  .Take(bootstrap.limit)
                                  .ToListAsync();

            if (bootstrap.order != null && bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                list.Reverse();
            }

            totalNumber = await query.CountAsync();

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }

        public async Task<long> GetRoleAsync(long userId)
        {
            Expression<Func<SysUser, bool>> userExpression = user => user.UserId == userId;
            var user = await _repository.QueryableToSingleAsync(userExpression);

            // 确保查询到了用户
            if (user == null)
            {
                throw new ArgumentException($"User with ID {userId} not found.");
            }

            // 获取用户的角色
            var roleId = user.RoleId;

            // 返回角色列表
            return roleId;
        }

        public async Task<bool> Insert(SysUser user, long userId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 设置创建者和创建日期
                    user.CreateBy = userId;
                    user.CreateDate = DateTime.UtcNow;

                    user.ModifiedBy = userId;
                    user.ModifiedDate = DateTime.UtcNow;
                    // 插入用户
                    _dbContext.Add(user);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> Update(SysUser user, long userId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 设置最后修改者和最后修改日期
                    user.ModifiedBy = userId;
                    user.ModifiedDate = DateTime.UtcNow;

                    // 更新用户
                    _dbContext.Update(user);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> Disable(SysUser user, long userId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 禁用用户
                    user.IsDel = 0; // 假设SysUser类中有IsEnabled属性
                    user.ModifiedBy = userId;
                    user.ModifiedDate = DateTime.UtcNow;

                    // 更新用户
                    _dbContext.Update(user);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> Delete(SysUser user)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // 删除用户
                    _dbContext.Remove(user);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

    }
}

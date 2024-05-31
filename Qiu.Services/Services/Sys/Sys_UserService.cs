using DB.Models;
using Microsoft.EntityFrameworkCore;
using Qiu.Utils.Extensions;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using Qiu.Utils.Json;
using Qiu.Utils.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using IServices.Sys;
using IResponsitory.Sys;

namespace Services.Sys
{
    public class Sys_UserService : BaseService<SysUser>, ISys_UserService
    {
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly ISys_UserResponsitory _repository;
        private readonly ISys_IdentityService _sys_identityservice;
        public Sys_UserService(
            QrsfactoryWmsContext dbContext,
            ISys_UserResponsitory repository,
            ISys_IdentityService sys_identityservice
            ) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
            _sys_identityservice = sys_identityservice;

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
                    s.Dept.DeptName,
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

    }
}

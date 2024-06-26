﻿using DB.Models;
using IRepository.Sys;
using IServices.Sys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Qiu.Utils.Extensions;
using Qiu.Utils.Table;
using SqlSugar.Extensions;
using System.Text.Json;

namespace Services.Sys
{
    public class Sys_LogService : BaseService<SysLog>, ISys_LogService
    {
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly IConfiguration _configuration;
        public Sys_LogService(
            IConfiguration configuration,
            QrsfactoryWmsContext dbContext,
            ISys_LogRepository repository
            ) : base(repository)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        //数据分页
        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            int totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<SysLog>()
                .Include(s => s.CreateByUser)
                .Include(s => s.ModifiedByUser)
                .Select(s => new
                {
                    LogId = s.LogId.ToString(),
                    s.LogIp,
                    s.LogType,
                    s.Url,
                    s.Browser,
                    s.Description,
                    CName = s.CreateByUser.UserNickname,
                    s.CreateDate,
                    UName = s.ModifiedByUser.UserNickname,
                    s.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(s => s.LogType.Contains(bootstrap.search));
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


        public async Task<string> EChart(Bootstrap.BootstrapParams bootstrap)
        {
            var query = _dbContext.Set<SysLog>()
                .Where(s => s.LogType == "login")
                .Where(s => s.CreateDate > bootstrap.datemin.ToDateTimeB() && s.CreateDate <= bootstrap.datemax.ToDateTimeE())
                .GroupBy(s => s.CreateDate.ObjToDate()) // Grouping by date without time
                .Select(g => new Log
                {
                    CreateDate = g.Key.ToString("yyyy-MM-dd"),
                    COUNT = g.Count()
                })
                .OrderBy(l => l.CreateDate);

            var list = await query.ToListAsync();

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(list);
        }


        public class Log
        {
            public required string CreateDate { get; set; }

            public int COUNT { get; set; }
        }

    }
}

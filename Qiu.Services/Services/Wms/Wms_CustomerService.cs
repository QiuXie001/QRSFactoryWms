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
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            int totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<WmsCustomer>()
                .Include(c => c.CreateByUser)
                .Include(c => c.ModifiedByUser)
                .Where(c => c.IsDel == 1)
                .Select(c => new
                {
                    CustomerId = c.CustomerId.ToString(),
                    c.CustomerNo,
                    c.CustomerName,
                    c.Address,
                    c.CustomerPerson,
                    c.CustomerLevel,
                    c.Tel,
                    c.Email,
                    c.IsDel,
                    c.Remark,
                    CName = c.CreateByUser.UserNickname,
                    c.CreateDate,
                    UName = c.ModifiedByUser.UserNickname,
                    c.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(c => c.CustomerName.Contains(bootstrap.search) || c.CustomerNo.Contains(bootstrap.search));
            }

            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query = query.Where(c => c.CreateDate > bootstrap.datemin.ToDateTimeB() && c.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }

            if (bootstrap.order != null && bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(c => EF.Property<object>(c, bootstrap.sort));
            }
            else
            {
                query = query.OrderBy(c => EF.Property<object>(c, bootstrap.sort));
            }

            var list = await query.Skip((pageNumber - 1) * bootstrap.limit)
                                  .Take(bootstrap.limit)
                                  .ToListAsync();

            totalNumber = await query.CountAsync();

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }


        public Task<(bool, string)> Import(DataTable dt, long userId)
        {
            throw new NotImplementedException();
        }

    }
}
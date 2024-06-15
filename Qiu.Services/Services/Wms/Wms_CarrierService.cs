using IRepository;
using IServices;
using Qiu.Utils.Table;
using DB.Models;
using System;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using IServices.Wms;
using IRepository.Wms;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Services
{
    public class Wms_CarrierService : BaseService<WmsCarrier>, IWms_CarrierService
    {
        private readonly IWms_CarrierRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;

        public Wms_CarrierService(QrsfactoryWmsContext dbContext, IWms_CarrierRepository repository) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            var totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<WmsCarrier>()
                .Include(s => s.CreateByUser)
                .Include(s => s.ModifiedByUser)
                .Where(s => s.IsDel == 1)
                .Select(s => new
                {
                    CarrierId = s.CarrierId.ToString(),
                    s.CarrierNo,
                    s.CarrierName,
                    s.Address,
                    s.CarrierPerson,
                    s.CarrierLevel,
                    s.Tel,
                    s.Email,
                    s.IsDel,
                    s.Remark,
                    CName = s.CreateByUser.UserNickname,
                    s.CreateDate,
                    UName = s.ModifiedByUser.UserNickname,
                    s.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(s => s.CarrierName.Contains(bootstrap.search) || s.CarrierNo.Contains(bootstrap.search));
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
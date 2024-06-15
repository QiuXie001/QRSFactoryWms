using IRepository;
using IServices;
using Microsoft.AspNetCore.Hosting;
using DB.Models;
using System;
using System.IO;
using System.Linq;
using Qiu.Utils.Check;
using Qiu.Utils.Extensions;
using Qiu.Utils.Json;
using Qiu.Utils.Pub;
using Qiu.Utils.Table;
using IRepository.Wms;
using IServices.Wms;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Services
{
    public class Wms_InventorymoveService : BaseService<WmsInventorymove>, IWms_InventorymoveService
    {
        private readonly IWms_InventorymoveRepository _repository;
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public Wms_InventorymoveService(IWms_InventorymoveRepository repository,
            QrsfactoryWmsContext dbContext,
            IWebHostEnvironment env
            ) : base(repository)
        {
            _repository = repository;
            _dbContext = dbContext;
            _env = env;
        }

        public async Task<string> PageListAsync(Bootstrap.BootstrapParams bootstrap)
        {
            int totalNumber = 0;
            int pageNumber = bootstrap.offset == 0 ? 1 : bootstrap.offset / bootstrap.limit + 1;

            var query = _dbContext.Set<WmsInventorymove>()
                .Include(w => w.SourceStoragerack)
                .Include(w => w.AimStoragerack)
                .Include(w => w.CreateByUser)
                .Include(w => w.ModifiedByUser)
                .Where(w => w.IsDel == 1)
                .Select(w => new
                {
                    InventorymoveId = w.InventorymoveId.ToString(),
                    w.InventorymoveNo,
                    w.Status,
                    SourceStorageRackNo = w.SourceStoragerack.StorageRackNo,
                    SourceStorageRackName = w.SourceStoragerack.StorageRackName,
                    AimStorageRackNo = w.AimStoragerack.StorageRackNo,
                    AimStorageRackName = w.AimStoragerack.StorageRackName,
                    w.IsDel,
                    w.Remark,
                    CName = w.CreateByUser.UserNickname,
                    w.CreateDate,
                    UName = w.ModifiedByUser.UserNickname,
                    w.ModifiedDate
                });

            if (!bootstrap.search.IsEmpty())
            {
                query = query.Where(w => w.InventorymoveNo.Contains(bootstrap.search));
            }

            if (!bootstrap.datemin.IsEmpty() && !bootstrap.datemax.IsEmpty())
            {
                query = query.Where(w => w.CreateDate > bootstrap.datemin.ToDateTimeB() && w.CreateDate <= bootstrap.datemax.ToDateTimeE());
            }

            if (bootstrap.order != null && bootstrap.order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(w => EF.Property<object>(w, bootstrap.sort));
            }
            else
            {
                query = query.OrderBy(w => EF.Property<object>(w, bootstrap.sort));
            }

            var list = await query.Skip((pageNumber - 1) * bootstrap.limit)
                                  .Take(bootstrap.limit)
                                  .ToListAsync();

            totalNumber = await query.CountAsync();

            // 使用 Newtonsoft.Json 或 System.Text.Json 进行 JSON 序列化
            return JsonSerializer.Serialize(new { rows = list, total = totalNumber });
        }


        public Task<bool> Auditin(long userId, long InventorymoveId)
        {
            throw new NotImplementedException();
        }


        public Task<string> PrintList(string InventorymoveId)
        {
            throw new NotImplementedException();
        }
    }
}
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
using Qiu.NetCore.DI;
using IServices.Wms;
using IRepository.Wms;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

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

        public async Task<(bool,string)> ImportAsync(System.Data.DataTable dt, long userId)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)
                {
                    return (false, PubConst.Import1);
                }
                var list = new List<WmsCustomer>();
                string[] header = { "客户编号", "客户名称", "电话", "邮箱", "联系人", "地址" };

                foreach (var item in header)
                {
                    if (!dt.Columns.Contains(item))
                    {
                        throw new ArgumentException("不包含Excel表头: " + string.Join(",", header));
                    }
                }

                int dtCount = dt.Rows.Count;
                for (int i = 0; i < dtCount; i++)
                {
                    var model = new WmsCustomer
                    {
                        CustomerNo = dt.Rows[i]["客户编号"].ToString(),
                        CustomerName = dt.Rows[i]["客户名称"].ToString(),
                        Address = dt.Rows[i]["地址"].ToString(),
                        Tel = dt.Rows[i]["电话"].ToString(),
                        Email = dt.Rows[i]["邮箱"].ToString(),
                        CustomerPerson = dt.Rows[i]["联系人"].ToString(),
                    };

                    if (await _repository.IsAnyAsync(c => c.CustomerNo == model.CustomerNo))
                    {
                        throw new Exception("客户编号已存在");
                    }

                    model.CustomerId = PubId.SnowflakeId;
                    model.CreateBy = userId;
                    list.Add(model);
                }

                // 开始事务
                var transaction = await _dbContext.Database.BeginTransactionAsync();

                // 执行批量插入
                var flag = await _repository.InsertBatchAsync(list);

                // 检查插入是否成功
                if (flag)
                {
                    // 提交事务
                    await transaction.CommitAsync();
                    return (true, PubConst.Import2);
                }
                else
                {
                    // 回滚事务
                    await transaction.RollbackAsync();
                    throw new Exception("导入失败: " + flag);
                }
            }
            catch (Exception ex)
            {
                // 使用日志记录服务记录错误
                var _nlog = ServiceResolve.Resolve<ILogUtil>();
                _nlog.Error("导入客户信息失败");
                return (false, PubConst.Import3);
            }
        }



    }
}
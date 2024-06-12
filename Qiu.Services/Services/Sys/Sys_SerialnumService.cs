using DB.Models;
using IRepository.Sys;
using IServices.Sys;
using Qiu.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Sys
{
    public class Sys_SerialnumService : BaseService<SysSerialnum>, ISys_SerialnumService
    {
        private readonly QrsfactoryWmsContext _dbContext;
        private readonly ISys_SerialnumRepository _repository;
        public Sys_SerialnumService(
            QrsfactoryWmsContext dbContext,
            ISys_SerialnumRepository repository
            ) : base(repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }
        public async Task<string> GetSerialnumAsync(long userId, string tableName)
        {
            var dt = DateTimeExt.GetDateTimeS(DateTimeExt.DateTimeShortFormat);
            var model = await _repository.QueryableToSingleAsync(c => c.TableName == tableName && c.IsDel == 1);
            if (dt == null || model.ModifiedDate == null || dt != model.ModifiedDate.Value.ToString(DateTimeExt.DateTimeShortFormat))
            {
                model.SerialCount = 0;
            }
            model.SerialCount++;
            var num = model.Prefix + DateTimeExt.GetDateTimeS(DateTimeExt.DateTimeFormatString) + model.SerialCount.ToString().PadLeft((int)model.Mantissa, '0');
            var flag =await _repository.UpdateAsync(new SysSerialnum { SerialNumberId = model.SerialNumberId, SerialNumber = num, SerialCount = model.SerialCount, ModifiedBy = userId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.SerialNumber, c.SerialCount, c.ModifiedBy, c.ModifiedDate });
            return num;
        }

        public async Task<SysSerialnum> GetSerialnumEntityAsync(long userId, string tableName)
        {
            var dt = DateTimeExt.GetDateTimeS(DateTimeExt.DateTimeShortFormat);
            var model =await _repository.QueryableToSingleAsync(c => c.TableName == tableName && c.IsDel == 1);
            if (dt == null || model.ModifiedDate == null || dt != model.ModifiedDate.Value.ToString(DateTimeExt.DateTimeShortFormat))
            {
                model.SerialCount = 0;
            }
            model.SerialCount++;
            var num = model.Prefix + DateTimeExt.GetDateTimeS(DateTimeExt.DateTimeFormatString) + model.SerialCount.ToString().PadLeft((int)model.Mantissa, '0');
            model.SerialNumber = num;
            await _repository.UpdateAsync(new SysSerialnum { SerialNumberId = model.SerialNumberId, SerialCount = model.SerialCount, ModifiedBy = userId, ModifiedDate = DateTimeExt.DateTime }, c => new { c.SerialNumber, c.SerialCount, c.ModifiedBy, c.ModifiedDate });
            return model;
        }
    }
}

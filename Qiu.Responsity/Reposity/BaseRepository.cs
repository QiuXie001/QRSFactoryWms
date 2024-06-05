using System.Data.Common;
using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using IRepository;
using System.Text.Json;
using System.Reflection;
using DB.Models;

namespace Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly QrsfactoryWmsContext _dbContext;

        public BaseRepository(QrsfactoryWmsContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #region Add Operations

        public async Task<bool> InsertAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertIgnoreNullColumnsAsync(TEntity entity)
        {
            // 这里可以添加逻辑来忽略空列
            await InsertAsync(entity);
            return true;
        }

        public async Task<bool> InsertIgnoreNullColumnsAsync(TEntity entity, params string[] columns)
        {
            // 这里可以添加逻辑来忽略指定的空列
            await InsertAsync(entity);
            return true;
        }

        public async Task<bool> InsertBatchAsync(List<TEntity> entities)
        {
            _dbContext.Set<TEntity>().AddRange(entities);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> InsertIgnoreNullColumnsBatchAsync(List<TEntity> entities)
        {
            // 这里可以添加逻辑来忽略所有空列
            await InsertBatchAsync(entities);
            return true;
        }

        public async Task<bool> InsertIgnoreNullColumnsBatchAsync(List<TEntity> entities, params string[] columns)
        {
            // 这里可以添加逻辑来忽略指定的空列
            await InsertBatchAsync(entities);
            return true;
        }

        #endregion Add Operations

        #region Update Operations

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> expression)
        {
            // 这里可以添加逻辑来更新满足条件的实体
            await UpdateAsync(entity);
            return true;
        }

        public async Task<bool> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> expression)
        {
            // 这里可以添加逻辑来更新实体的指定属性
            await UpdateAsync(entity);
            return true;
        }

        public async Task<bool> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> expression, Expression<Func<TEntity, bool>> where)
        {
            // 这里可以添加逻辑来更新满足特定条件的实体属性
            await UpdateAsync(entity);
            return true;
        }

        public async Task<bool> UpdateAsync(List<TEntity> entities)
        {
            // 这里可以添加逻辑来批量更新实体列表
            await UpdateAsync(entities.FirstOrDefault());
            return true;
        }

        #endregion Update Operations

        #region Delete Operations

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            var entities = _dbContext.Set<TEntity>().Where(expression);
            foreach (var entity in entities)
            {
                _dbContext.Set<TEntity>().Remove(entity);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(params object[] primaryKeyValues)
        {
            var entity = _dbContext.Set<TEntity>().Find(primaryKeyValues);
            if (entity != null)
            {
                _dbContext.Set<TEntity>().Remove(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        #endregion Delete Operations

        #region Query Operations

        public async Task<bool> IsAnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbContext.Set<TEntity>().AnyAsync(expression);
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbContext.Set<TEntity>();
        }

        public async Task<List<TEntity>> QueryableToListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbContext.Set<TEntity>().Where(expression).ToListAsync();
        }
        public async Task<TEntity> QueryableToSingleAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbContext.Set<TEntity>().Where(expression).SingleOrDefaultAsync();
        }


        public async Task<string> QueryableToJsonAsync(string select, Expression<Func<TEntity, bool>> expressionWhere)
        {
            // 应用表达式筛选查询
            IQueryable<TEntity> queryable = _dbContext.Set<TEntity>().Where(expressionWhere);

            // 如果提供了 select 参数，尝试应用选择表达式
            // 这里需要解析 select 参数并应用选择表达式
            // 由于 select 参数的具体格式和解析方式可能因实现而异，这里只是提供了一个占位符
            // 您需要根据实际情况实现选择逻辑
            // queryable = queryable.Select(selectExpression);

            // 使用 IAsyncEnumerable 进行异步序列化
            await using (var asyncEnumerator = queryable.AsAsyncEnumerable().GetAsyncEnumerator())
            {
                var results = new List<TEntity>();
                while (await asyncEnumerator.MoveNextAsync())
                {
                    results.Add(asyncEnumerator.Current);
                }

                // 将结果列表转换为 JSON 字符串
                string json = JsonSerializer.Serialize(results);

                return json;
            }
        }

        public async Task<List<TEntity>> QueryableToListAsync(string tableName)
        {
            return await _dbContext.Set<TEntity>(tableName).ToListAsync();
        }

        public async Task<List<TEntity>> QueryableToListAsync(string tableName, Expression<Func<TEntity, bool>> expression)
        {
            return await _dbContext.Set<TEntity>(tableName).Where(expression).ToListAsync();
        }

        public async Task<(List<TEntity>, int)> QueryableToPageAsync(Expression<Func<TEntity, bool>> expression, int pageIndex = 0, int pageSize = 10)
        {
            var totalNumber = await _dbContext.Set<TEntity>().CountAsync(expression);
            var list = await _dbContext.Set<TEntity>().Where(expression).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            return (list, totalNumber);
        }

        public async Task<(List<TEntity>, int)> QueryableToPageAsync(Expression<Func<TEntity, bool>> expression, string order, int pageIndex = 0, int pageSize = 10)
        {
            // 应用表达式筛选查询
            IQueryable<TEntity> queryable = _dbContext.Set<TEntity>().Where(expression);

            // 应用排序表达式
            if (!string.IsNullOrEmpty(order))
            {
                PropertyInfo propertyInfo = typeof(TEntity).GetProperty(order);
                queryable = queryable.OrderBy(item => propertyInfo.GetValue(item));
            }

            // 获取总记录数
            int totalCount = await queryable.CountAsync();

            // 获取当前页的数据
            queryable = queryable.Skip(pageIndex * pageSize);
            queryable = queryable.Take(pageSize);
            List<TEntity> currentPage = await queryable.ToListAsync();

            return (currentPage, totalCount);
        }

        public async Task<(List<TEntity>, int)> QueryableToPageAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderBy, string orderByDirection, int pageIndex = 0, int pageSize = 10)
        {
            var totalNumber = await _dbContext.Set<TEntity>().CountAsync(expression);
            var list = await _dbContext.Set<TEntity>().Where(expression).OrderBy(orderBy).Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
            if (orderByDirection == "desc")
            { 
                list.Reverse();
            }
            return (list, totalNumber);
        }

        public async Task<List<TEntity>> SqlQueryToListAsync(string sql, object parameters = null)
        {
            var results = await _dbContext.Database.SqlQueryRaw<TEntity>(sql, parameters).ToListAsync();
            return results;
        }

        #endregion Query Operations

        #region Transaction Operations
        public async Task<bool> UseTransactionAsync(Func<Task> action)
        {
            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await action();
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // 处理异常
                    return false;
                }
            }
        }

        public async Task<bool> UseTransactionAsync(List<TEntity> entities, Func<Task> action)
        {
            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.Set<TEntity>().AddRange(entities);
                    await action();
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // 处理异常
                    return false;
                }
            }
        }

        #endregion Transaction Operations

        #region Stored Procedure Operations

        public async Task<DataTable> UseStoredProcedureToDataTableAsync(string procedureName, params DbParameter[] parameters)
        {
            return (DataTable)await Task.FromResult(_dbContext.Database.SqlQueryRaw<TEntity>(procedureName, parameters));
        }

        public Task<(DataTable, DbParameter[])> UseStoredProcedureToTupleAsync(string procedureName, params DbParameter[] parameters)
        {
            var dataTable = (DataTable)_dbContext.Database.SqlQueryRaw<TEntity>(procedureName, parameters);
            var outputParameters = parameters.Where(p => p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput).ToArray();
            return Task.FromResult((dataTable, outputParameters));
        }

        #endregion Stored Procedure Operations
    }
}

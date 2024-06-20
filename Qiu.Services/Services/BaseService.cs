using IRepository;
using IServices;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        public IBaseRepository<TEntity> _baseRepository;
        public BaseService(IBaseRepository<TEntity> repository)
        {
            _baseRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        #region Add Operations

        public async Task<bool> InsertAsync(TEntity entity)
        {
            return await _baseRepository.InsertAsync(entity);
        }

        public async Task<bool> InsertIgnoreNullColumnsAsync(TEntity entity)
        {
            return await _baseRepository.InsertIgnoreNullColumnsAsync(entity);
        }

        public async Task<bool> InsertIgnoreNullColumnsAsync(TEntity entity, params string[] columns)
        {
            return await _baseRepository.InsertIgnoreNullColumnsAsync(entity, columns);
        }

        public async Task<bool> InsertBatchAsync(List<TEntity> entities)
        {
            return await _baseRepository.InsertBatchAsync(entities);
        }

        public async Task<bool> InsertIgnoreNullColumnsBatchAsync(List<TEntity> entities)
        {
            return await _baseRepository.InsertIgnoreNullColumnsBatchAsync(entities);
        }

        public async Task<bool> InsertIgnoreNullColumnsBatchAsync(List<TEntity> entities, params string[] columns)
        {
            return await _baseRepository.InsertIgnoreNullColumnsBatchAsync(entities, columns);
        }



        #endregion Add Operations

        #region Update Operations

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            return await _baseRepository.UpdateAsync(entity);
        }

        public async Task<bool> UpdateAsync(TEntity entity, Expression<Func<TEntity, bool>> expression)
        {
            return await _baseRepository.UpdateAsync(entity, expression);
        }

        public async Task<bool> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> expression)
        {
            return await _baseRepository.UpdateAsync(entity, expression);
        }

        public async Task<bool> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> expression, Expression<Func<TEntity, bool>> where)
        {
            return await _baseRepository.UpdateAsync(entity, expression, where);
        }

        public async Task<bool> UpdateAsync(List<TEntity> entities)
        {
            return await _baseRepository.UpdateAsync(entities);
        }

        #endregion Update Operations

        #region Delete Operations

        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _baseRepository.DeleteAsync(expression);
        }

        public async Task<bool> DeleteAsync(params object[] primaryKeyValues)
        {
            return await _baseRepository.DeleteAsync(primaryKeyValues);
        }


        #endregion Delete Operations

        #region Query Operations

        public async Task<bool> IsAnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _baseRepository.IsAnyAsync(expression);
        }

        public IQueryable<TEntity> Queryable()
        {
            return _baseRepository.Queryable();
        }

        public async Task<List<TEntity>> QueryableToListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _baseRepository.QueryableToListAsync(expression);
        }

        public async Task<TEntity> QueryableToSingleAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _baseRepository.QueryableToSingleAsync(expression);
        }

        public async Task<string> QueryableToJsonAsync(string select, Expression<Func<TEntity, bool>> expressionWhere)
        {
            return await _baseRepository.QueryableToJsonAsync(select, expressionWhere);
        }

        public async Task<List<TEntity>> QueryableToListAsync(string tableName)
        {
            return await _baseRepository.QueryableToListAsync(tableName);
        }

        public async Task<List<TEntity>> QueryableToListAsync(string tableName, Expression<Func<TEntity, bool>> expression)
        {
            return await _baseRepository.QueryableToListAsync(tableName, expression);
        }

        public async Task<(List<TEntity>, int)> QueryableToPageAsync(Expression<Func<TEntity, bool>> expression, int pageIndex = 0, int pageSize = 10)
        {
            return await _baseRepository.QueryableToPageAsync(expression, pageIndex, pageSize);
        }

        public async Task<(List<TEntity>, int)> QueryableToPageAsync(Expression<Func<TEntity, bool>> expression, string order, int pageIndex = 0, int pageSize = 10)
        {
            return await _baseRepository.QueryableToPageAsync(expression, order, pageIndex, pageSize);
        }

        public async Task<(List<TEntity>, int)> QueryableToPageAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>> orderBy, string orderByDirection, int pageIndex = 0, int pageSize = 10)
        {
            return await _baseRepository.QueryableToPageAsync(expression, orderBy, orderByDirection, pageIndex, pageSize);
        }

        public async Task<List<TEntity>> SqlQueryToListAsync(string sql, object parameters = null)
        {
            return await _baseRepository.SqlQueryToListAsync(sql, parameters);
        }



        #endregion Query Operations

        #region Transaction Operations
        public async Task<bool> UseTransactionAsync(Func<Task> action)
        {
            return await _baseRepository.UseTransactionAsync(action);
        }

        public async Task<bool> UseTransactionAsync(List<TEntity> entities, Func<Task> action)
        {
            return await _baseRepository.UseTransactionAsync(entities, action);
        }


        #endregion Transaction Operations

        #region Stored Procedure Operations

        public async Task<DataTable> UseStoredProcedureToDataTableAsync(string procedureName, params DbParameter[] parameters)
        {
            return await _baseRepository.UseStoredProcedureToDataTableAsync(procedureName, parameters);
        }

        public async Task<(DataTable, DbParameter[])> UseStoredProcedureToTupleAsync(string procedureName, params DbParameter[] parameters)
        {
            return await _baseRepository.UseStoredProcedureToTupleAsync(procedureName, parameters);
        }

        #endregion Stored Procedure Operations
    }
}

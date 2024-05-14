using System.Data.Common;
using System.Data;
using System.Linq.Expressions;

namespace IResponsitory
{
    public interface IBaseResponsitory<T> where T : class, new()
    {

        #region Add Operations

        Task<bool> InsertAsync(T entity);

        Task<bool> InsertIgnoreNullColumnsAsync(T entity);

        Task<bool> InsertIgnoreNullColumnsAsync(T entity, params string[] columns);

        Task<bool> InsertBatchAsync(List<T> entities);

        Task<bool> InsertIgnoreNullColumnsBatchAsync(List<T> entities);

        Task<bool> InsertIgnoreNullColumnsBatchAsync(List<T> entities, params string[] columns);

        #endregion Add Operations

        #region Update Operations

        Task<bool> UpdateAsync(T entity);

        Task<bool> UpdateAsync(T entity, Expression<Func<T, bool>> expression);

        Task<bool> UpdateAsync(T entity, Expression<Func<T, object>> expression);

        Task<bool> UpdateAsync(T entity, Expression<Func<T, object>> expression, Expression<Func<T, bool>> where);

        Task<bool> UpdateAsync(List<T> entities);

        #endregion Update Operations

        #region Delete Operations

        Task<bool> DeleteAsync(Expression<Func<T, bool>> expression);

        Task<bool> DeleteAsync(params object[] primaryKeyValues);

        #endregion Delete Operations

        #region Query Operations

        Task<bool> IsAnyAsync(Expression<Func<T, bool>> expression);

        IQueryable<T> Queryable();

        Task<List<T>> QueryableToListAsync(Expression<Func<T, bool>> expression);

        Task<T> QueryableToSingleAsync(Expression<Func<T, bool>> expression);

        Task<string> QueryableToJsonAsync(string select, Expression<Func<T, bool>> expressionWhere);

        Task<List<T>> QueryableToListAsync(string tableName);

        Task<List<T>> QueryableToListAsync(string tableName, Expression<Func<T, bool>> expression);

        Task<(List<T>, int)> QueryableToPageAsync(Expression<Func<T, bool>> expression, int pageIndex = 0, int pageSize = 10);

        Task<(List<T>, int)> QueryableToPageAsync(Expression<Func<T, bool>> expression, string order, int pageIndex = 0, int pageSize = 10);

        Task<(List<T>, int)> QueryableToPageAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderBy, string orderByDirection, int pageIndex = 0, int pageSize = 10);

        Task<List<T>> SqlQueryToListAsync(string sql, object parameters = null);

        #endregion Query Operations

        #region Transaction Operations

        Task<bool> UseTransactionAsync(Func<Task> action);

        Task<bool> UseTransactionAsync(List<T> entities, Func<Task> action);

        #endregion Transaction Operations

        #region Stored Procedure Operations

        Task<DataTable> UseStoredProcedureToDataTableAsync(string procedureName, params DbParameter[] parameters);

        Task<(DataTable, DbParameter[])> UseStoredProcedureToTupleAsync(string procedureName, params DbParameter[] parameters);

        #endregion Stored Procedure Operations
    }
}

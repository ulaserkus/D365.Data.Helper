using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D365.Data.Helper.Sql
{
    public interface ISqlService<TEntity> where TEntity : class
    {

        Task<List<TEntity>> GetAllEntitiesAsync(string entityName);
        Task<TEntity> GetEntityByIdAsync(Guid entityId,string entityName);
        Task<List<TEntity>> GetAllEntitiesWithColumnsAsync(string entityName, string[] columns);
        Task CreateEntityAsync(TEntity entity);
        Task UpdateEntityAsync(TEntity entity);
        Task DeleteEntityAsync(TEntity entity);
        Task<object> ExecuteQueryAsync(string query);
    }
}

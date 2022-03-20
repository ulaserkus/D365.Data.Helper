using D365.Data.Helper.CrmObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D365.Data.Helper.Online.IEntity
{
    public interface IOnlineEntityService : IOnlineService<Entity>
    {
        Task<Entity> GetEntityByIdAsync(string entityId, string entityName, string[] Columns);

        Task<Guid> CreateEntityAsync(Entity entity);

        Task<bool> UpdateEntityAsync(Entity entity);
    }
}

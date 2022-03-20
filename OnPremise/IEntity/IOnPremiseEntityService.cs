using D365.Data.Helper.CrmObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D365.Data.Helper.OnPremise.IEntity
{
    public interface IOnPremiseEntityService:IOnPremiseService<Entity>
    {
        Task<Entity> GetEntityByIdAsync(string entityId, string entityName, string[] Columns);

        Task<Guid> CreateEntityAsync(Entity entity);

        Task<bool> UpdateEntityAsync(Entity entity);

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D365.Data.Helper.Online
{
    public interface IOnlineService<TObject>
    {
        Task<List<TObject>> GetAllEntitiesAsync(string entityName);

        Task<TObject> GetEntityByIdAsync(string entityId, string entityName);

        Task<TObject> CreateEntityAsync(TObject TObject, string entityName);

        Task<bool> UpdateEntityAsync(TObject TObject, string entityName, string entityId);

        Task<bool> DeleteEntityAsync(string entityName, string entityId);

        Task<List<TObject>> GetEntitiesByFetchXmlAsync(string entityName, string fetchXml);
    }
}

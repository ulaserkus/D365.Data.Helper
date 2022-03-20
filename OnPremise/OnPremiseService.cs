using D365.Data.Helper.CRM;
using D365.Data.Helper.CrmObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace D365.Data.Helper.OnPremise
{
    public class OnPremiseService<TObject> : IOnPremiseService<TObject>
    {
        internal readonly CrmInfo crmInfo;

        public OnPremiseService(CrmInfo _crmInfo)
        {
            crmInfo = _crmInfo;
        }

        public async Task<TObject> CreateEntityAsync(TObject TObject, string entityName)
        {
            try
            {
                Pluralize.NET.Pluralizer pluralizer = new Pluralize.NET.Pluralizer();
                string entityPluralName = pluralizer.Pluralize(entityName);
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler() { Credentials = new NetworkCredential(crmInfo.UserName, crmInfo.Password) }))
                {
                    httpClient.BaseAddress = new Uri(crmInfo.ApiUrl);
                    httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");

                    StringContent content;

                    if (typeof(TObject) != typeof(Entity))
                        content = new StringContent(JsonConvert.SerializeObject(TObject), System.Text.Encoding.UTF8, "application/json");
                    else
                    {
                        Entity entity = (Entity)Convert.ChangeType(TObject, typeof(Entity));
                        entity.ConvertToOData(ref entity.Attributes);
                        content = new StringContent(JsonConvert.SerializeObject(entity.Attributes), System.Text.Encoding.UTF8, "application/json");

                    }


                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, entityPluralName);
                    requestMessage.Content = content;

                    HttpResponseMessage response = await Task.Run(async () => await httpClient.SendAsync(requestMessage));

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        if (typeof(TObject) != typeof(Entity))
                        {
                            TObject @object = JsonConvert.DeserializeObject<TObject>(await response.Content.ReadAsStringAsync());
                            return @object;
                        }
                        else
                        {
                            Entity entity = new Entity(entityName);
                            Dictionary<string, object> attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(await response.Content.ReadAsStringAsync());
                            if (attributes.TryGetValue(entity.LogicalName + "id", out object id))
                            {
                                entity.Id = Guid.Parse(id.ToString());
                                entity.Attributes = attributes;
                                entity.CleanNullValues(ref entity.Attributes);
                                entity.ConvertToEntityReference(ref entity.Attributes);

                                return (TObject)Convert.ChangeType(entity, typeof(TObject));
                            }
                        }
                    }

                }

                return (TObject)Activator.CreateInstance(typeof(TObject));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> DeleteEntityAsync(string entityName, string entityId)
        {
            try
            {
                Pluralize.NET.Pluralizer pluralizer = new Pluralize.NET.Pluralizer();
                string entityPluralName = pluralizer.Pluralize(entityName);
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler() { Credentials = new NetworkCredential(crmInfo.UserName, crmInfo.Password) }))
                {
                    httpClient.BaseAddress = new Uri(crmInfo.ApiUrl);

                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, entityPluralName + $"({entityId})");

                    HttpResponseMessage response = await Task.Run(async () => await httpClient.SendAsync(requestMessage));

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return true;
                    }

                }

                return false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<TObject>> GetAllEntitiesAsync(string entityName)
        {
            try
            {
                Pluralize.NET.Pluralizer pluralizer = new Pluralize.NET.Pluralizer();
                string entityPluralName = pluralizer.Pluralize(entityName);
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler() { Credentials = new NetworkCredential(crmInfo.UserName, crmInfo.Password) }))
                {
                    httpClient.BaseAddress = new Uri(crmInfo.ApiUrl);

                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, entityPluralName);

                    HttpResponseMessage response = await Task.Run(async () => await httpClient.SendAsync(requestMessage));

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        if (typeof(TObject) != typeof(Entity))
                        {
                            JObject json = JObject.Parse(content);
                            var model = JsonConvert.DeserializeObject<List<TObject>>(json["value"].ToString());

                            return model;
                        }
                        else
                        {
                            JObject json = JObject.Parse(content);
                            var dicList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json["value"].ToString());
                            List<Entity> entities = new List<Entity>();

                            foreach (var item in dicList)
                            {

                                Entity entity = new Entity(entityName);

                                if (item.TryGetValue(entityName + "id", out object val)) entity.Id = Guid.Parse(val.ToString());
                                entity.Attributes = item;
                                entity.CleanNullValues(ref entity.Attributes);
                                entity.ConvertToEntityReference(ref entity.Attributes);
                                entities.Add(entity);
                            }

                            return entities as List<TObject>;
                        }

                    }

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<List<TObject>> GetEntitiesByFetchXmlAsync(string entityName, string fetchXml)
        {
            try
            {
                Pluralize.NET.Pluralizer pluralizer = new Pluralize.NET.Pluralizer();
                string entityPluralName = pluralizer.Pluralize(entityName);
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler() { Credentials = new NetworkCredential(crmInfo.UserName, crmInfo.Password) }))
                {
                    httpClient.BaseAddress = new Uri(crmInfo.ApiUrl);

                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, entityPluralName + "?fetchXml=" + Uri.EscapeDataString(fetchXml));

                    HttpResponseMessage response = await Task.Run(async () => await httpClient.SendAsync(requestMessage));

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();

                        if (typeof(TObject) != typeof(Entity))
                        {
                            JObject json = JObject.Parse(content);
                            var model = JsonConvert.DeserializeObject<List<TObject>>(json["value"].ToString());

                            return model;
                        }
                        else
                        {
                            JObject json = JObject.Parse(content);
                            var dicList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json["value"].ToString());
                            List<Entity> entities = new List<Entity>();

                            foreach (var item in dicList)
                            {

                                Entity entity = new Entity(entityName);

                                if (item.TryGetValue(entityName + "id", out object val)) entity.Id = Guid.Parse(val.ToString());
                                entity.Attributes = item;
                                entity.CleanNullValues(ref entity.Attributes);
                                entity.ConvertToEntityReference(ref entity.Attributes);
                                entities.Add(entity);
                            }

                            return entities as List<TObject>;
                        }
                    }

                }

                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<TObject> GetEntityByIdAsync(string entityId, string entityName)
        {
            try
            {
                Pluralize.NET.Pluralizer pluralizer = new Pluralize.NET.Pluralizer();
                string entityPluralName = pluralizer.Pluralize(entityName);
         
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler() { Credentials = new NetworkCredential(crmInfo.UserName, crmInfo.Password) }))
                {
                    httpClient.BaseAddress = new Uri(crmInfo.ApiUrl);

                    HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, entityPluralName + $"({entityId})");

                    HttpResponseMessage response = await Task.Run(async () => await httpClient.SendAsync(requestMessage));

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        JObject json = JObject.Parse(content);

                        if (typeof(TObject) != typeof(Entity))
                        {
                            return JsonConvert.DeserializeObject<TObject>(json.ToString());
                        }
                        else
                        {
                            Entity entity = new Entity(entityName);
                            entity.Id = Guid.Parse(entityId);
                            Dictionary<string, object> attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.ToString());
                            entity.Attributes = attributes;
                            entity.CleanNullValues(ref entity.Attributes);
                            entity.ConvertToEntityReference(ref entity.Attributes);
                            return (TObject)Convert.ChangeType(entity, typeof(TObject));
                        }
                    }

                    return (TObject)Activator.CreateInstance(typeof(TObject));

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> UpdateEntityAsync(TObject TObject, string entityName, string entityId)
        {
            try
            {
                Pluralize.NET.Pluralizer pluralizer = new Pluralize.NET.Pluralizer();
                string entityPluralName = pluralizer.Pluralize(entityName);
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler() { Credentials = new NetworkCredential(crmInfo.UserName, crmInfo.Password) }))
                {
                    httpClient.BaseAddress = new Uri(crmInfo.ApiUrl);

                    StringContent content;

                    if (typeof(TObject) != typeof(Entity))
                        content = new StringContent(JsonConvert.SerializeObject(TObject), System.Text.Encoding.UTF8, "application/json");
                    else
                    {
                        Entity entity = (Entity)Convert.ChangeType(TObject, typeof(Entity));
                        entity.ConvertToOData(ref entity.Attributes);
                        content = new StringContent(JsonConvert.SerializeObject(entity.Attributes), System.Text.Encoding.UTF8, "application/json");
                    }

                    HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), entityPluralName + $"({entityId})");
                    requestMessage.Content = content;

                    HttpResponseMessage response = await Task.Run(async () => await httpClient.SendAsync(requestMessage));

                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return true;
                    }
                }

                return false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

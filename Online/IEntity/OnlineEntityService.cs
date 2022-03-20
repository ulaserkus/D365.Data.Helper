using D365.Data.Helper.CRM;
using D365.Data.Helper.CrmObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace D365.Data.Helper.Online.IEntity
{
    public class OnlineEntityService : OnlineService<Entity>, IOnlineEntityService
    {
        public OnlineEntityService(CrmInfo crmInfo) : base(crmInfo)
        {

        }

        public async Task<Entity> GetEntityByIdAsync(string entityId, string entityName, string[] Columns)
        {
            try
            {
                string access_token = await GetAccessToken();
                Pluralize.NET.Pluralizer pluralizer = new Pluralize.NET.Pluralizer();
                string entityPluralName = pluralizer.Pluralize(entityName);

                if (access_token != string.Empty)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(crmInfo.ApiUrl);

                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, entityPluralName + $"({entityId})");

                        HttpResponseMessage response = await Task.Run(async () => await httpClient.SendAsync(requestMessage));

                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            JObject json = JObject.Parse(content);
                            Entity entity = new Entity(entityName);
                            entity.Id = Guid.Parse(entityId);
                            Dictionary<string, object> returnedAttributes = new Dictionary<string, object>();
                            Dictionary<string, object> attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(json.ToString());

                            foreach (var attribute in attributes)
                            {
                                if (Columns.ToList().Contains(attribute.Key))
                                {
                                    returnedAttributes.Add(attribute.Key, attribute.Value);
                                }

                            }

                            entity.Attributes = returnedAttributes;
                            entity.CleanNullValues(ref entity.Attributes);
                            entity.ConvertToEntityReference(ref entity.Attributes);
                            return entity;


                        }

                        return new Entity(entityName);

                    }
                }

                return new Entity(entityName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
         
        }

        public async Task<bool> UpdateEntityAsync(Entity entity)
        {
            try
            {
                string access_token = await GetAccessToken();
                Pluralize.NET.Pluralizer pluralizer = new Pluralize.NET.Pluralizer();
                string entityPluralName = pluralizer.Pluralize(entity.LogicalName);

                if (access_token != string.Empty)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(crmInfo.ApiUrl);

                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                        entity.ConvertToOData(ref entity.Attributes);

                        var content = new StringContent(JsonConvert.SerializeObject(entity.Attributes), System.Text.Encoding.UTF8, "application/json");

                        HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), entityPluralName + $"({entity.Id})");
                        requestMessage.Content = content;

                        HttpResponseMessage response = await Task.Run(async () => await httpClient.SendAsync(requestMessage));

                        if (response.StatusCode == HttpStatusCode.NoContent)
                        {
                            return true;
                        }

                    }
                }

                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<Guid> CreateEntityAsync(Entity entity)
        {
            try
            {
                string access_token = await GetAccessToken();
                Pluralize.NET.Pluralizer pluralizer = new Pluralize.NET.Pluralizer();
                string entityPluralName = pluralizer.Pluralize(entity.LogicalName);

                if (access_token != string.Empty)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(crmInfo.ApiUrl);

                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
                        httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");
                        entity.ConvertToOData(ref entity.Attributes);
                        var content = new StringContent(JsonConvert.SerializeObject(entity.Attributes), System.Text.Encoding.UTF8, "application/json");

                        HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, entityPluralName);
                        requestMessage.Content = content;

                        HttpResponseMessage response = await Task.Run(async () => await httpClient.SendAsync(requestMessage));

                        if (response.StatusCode == HttpStatusCode.Created)
                        {
                            Dictionary<string, object> attributes = JsonConvert.DeserializeObject<Dictionary<string, object>>(await response.Content.ReadAsStringAsync());
                            if (attributes.TryGetValue(entity.LogicalName + "id", out object id))
                            {
                                return Guid.Parse(id.ToString());
                            }

                            return Guid.Empty; ;
                        }

                    }
                }

                return Guid.Empty;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

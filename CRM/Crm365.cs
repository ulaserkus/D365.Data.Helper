using D365.Data.Helper.Online;
using D365.Data.Helper.Online.IEntity;
using D365.Data.Helper.OnPremise;
using D365.Data.Helper.OnPremise.IEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace D365.Data.Helper.CRM
{
    public class Crm365
    {
        private Container Container { get; set; }
        private CrmInfo crmInfo { get; set; }

        public Crm365(string connectionString)
        {
            Container = new Container();
            List<string> connectionInfos = connectionString.Split(';').ToList();
            string apiUrl = string.Empty;
            string username = string.Empty;
            string password = string.Empty;
            string resource = string.Empty;
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string tenantId = string.Empty;

            if (connectionInfos.Where(x => x.Split('=')[0].Contains("apiUrl") || x.Split('=')[0].Contains("ApiUrl")).Any())
                apiUrl = connectionInfos.Where(x => x.Split('=')[0].Contains("apiUrl") || x.Split('=')[0].Contains("ApiUrl")).FirstOrDefault().Split('=')[1].Replace(" ", "");

            if (connectionInfos.Where(x => x.Split('=')[0].Contains("userName") || x.Split('=')[0].Contains("UserName")).Any())
                username = connectionInfos.Where(x => x.Split('=')[0].Contains("userName") || x.Split('=')[0].Contains("UserName")).FirstOrDefault().Split('=')[1].Replace(" ", "");

            if (connectionInfos.Where(x => x.Split('=')[0].Contains("password") || x.Split('=')[0].Contains("Password")).Any())
                password = connectionInfos.Where(x => x.Split('=')[0].Contains("password") || x.Split('=')[0].Contains("Password")).FirstOrDefault().Split('=')[1].Replace(" ", "");

            if (connectionInfos.Where(x => x.Split('=')[0].Contains("resource") || x.Split('=')[0].Contains("Resource")).Any())
                resource = connectionInfos.Where(x => x.Split('=')[0].Contains("resource") || x.Split('=')[0].Contains("Resource")).FirstOrDefault().Split('=')[1].Replace(" ", "");

            if (connectionInfos.Where(x => x.Split('=')[0].Contains("clientId") || x.Split('=')[0].Contains("ClientId")).Any())
                clientId = connectionInfos.Where(x => x.Split('=')[0].Contains("clientId") || x.Split('=')[0].Contains("ClientId")).FirstOrDefault().Split('=')[1].Replace(" ", "");

            if (connectionInfos.Where(x => x.Split('=')[0].Contains("clientSecret") || x.Split('=')[0].Contains("ClientSecret")).Any())
                clientSecret = connectionInfos.Where(x => x.Split('=')[0].Contains("clientSecret") || x.Split('=')[0].Contains("ClientSecret")).FirstOrDefault().Split('=')[1].Replace(" ", "");

            if (connectionInfos.Where(x => x.Split('=')[0].Contains("tenantId") || x.Split('=')[0].Contains("TenantId")).Any())
                tenantId = connectionInfos.Where(x => x.Split('=')[0].Contains("tenantId") || x.Split('=')[0].Contains("TenantId")).FirstOrDefault().Split('=')[1].Replace(" ", "");

            if (!string.IsNullOrEmpty(apiUrl) && !string.IsNullOrEmpty(tenantId) && !string.IsNullOrEmpty(clientSecret) && !string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(resource))
            {
                crmInfo = new CrmInfo
                {
                    ApiUrl = apiUrl,
                    UserName = username,
                    Password = password,
                    Resource = resource,
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    TenantId = tenantId,
                    CrmType = ECrmType.Online

                };

            }
            else if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(apiUrl))
            {
                crmInfo = new CrmInfo
                {
                    ApiUrl = apiUrl,
                    UserName = username,
                    Password = password,
                    Resource = resource,
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    TenantId = tenantId,
                    CrmType = ECrmType.OnPremise

                };
            }
            else
            {
                throw new Exception("Invalid ConnectionString");
            }
        }


        public IOnlineEntityService GetOnlineService()
        {
            if (!Container.HasKey(typeof(IOnlineEntityService)))
                Container.Register<IOnlineEntityService, OnlineEntityService>(crmInfo);

            IOnlineEntityService onlineEntityService = (IOnlineEntityService)Container.GetInstance(typeof(IOnlineEntityService), crmInfo);
            if (crmInfo.CrmType == ECrmType.Online)
                return onlineEntityService;
            else throw new Exception("Service and crm type do not match");
        }

        public IOnlineService<TEntity> GetOnlineService<TEntity>()
        {
            if (!Container.HasKey(typeof(IOnlineService<TEntity>)))
                Container.Register<IOnlineService<TEntity>, OnlineService<TEntity>>(crmInfo);

            IOnlineService<TEntity> onlineService = (IOnlineService<TEntity>)Container.GetInstance(typeof(IOnlineEntityService), crmInfo);
            if (crmInfo.CrmType == ECrmType.Online)
                return onlineService;
            else throw new Exception("Service and crm type do not match");
        }

        public IOnPremiseEntityService GetOnPremiseService()
        {
            if (!Container.HasKey(typeof(IOnPremiseEntityService)))
                Container.Register<IOnPremiseEntityService, OnPremiseEntityService>(crmInfo);

            IOnPremiseEntityService onPremiseEntityService = (IOnPremiseEntityService)Container.GetInstance(typeof(IOnPremiseEntityService), crmInfo);
            if (crmInfo.CrmType == ECrmType.OnPremise)
                return onPremiseEntityService;
            else throw new Exception("Service and crm type do not match");
        }

        public IOnPremiseService<TEntity> GetOnPremiseService<TEntity>()
        {
            if (!Container.HasKey(typeof(IOnPremiseService<TEntity>)))
                Container.Register<IOnPremiseService<TEntity>, OnPremiseService<TEntity>>(crmInfo);

            IOnPremiseService<TEntity> onPremiseService = (IOnPremiseService<TEntity>)Container.GetInstance(typeof(IOnPremiseService<TEntity>), crmInfo);
            if (crmInfo.CrmType == ECrmType.OnPremise)
                return onPremiseService;
            else throw new Exception("Service and crm type do not match");
        }
    }
}

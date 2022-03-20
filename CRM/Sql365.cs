using System.Collections.Generic;
using System.Linq;

namespace D365.Data.Helper.CRM
{
    public class Sql365
    {
        private Container Container { get; set; }
        private SqlInfo sqlInfo { get; set; }

        public Sql365(string connectionString)
        {
            Container = new Container();
            List<string> connectionInfos = connectionString.Split(';').ToList();
            string server = string.Empty;
            string username = string.Empty;
            string password = string.Empty;
            string database = string.Empty;
            ESqlConnectionType sqlConnectionType = ESqlConnectionType.defaultType;


            if (connectionInfos.Where(x => x.Split('=')[0].Contains("Data Source") || x.Split('=')[0].Contains("data source") || x.Split('=')[0].Contains("server") || x.Split('=')[0].Contains("Server")).Any())
                server = connectionInfos.Where(x => x.Split('=')[0].Contains("data source") || x.Split('=')[0].Contains("Data Source") || x.Split('=')[0].Contains("server") || x.Split('=')[0].Contains("Server")).FirstOrDefault().Split('=')[1].Replace(" ", "");

            if (connectionInfos.Where(x => x.Split('=')[0].Contains("userName") || x.Split('=')[0].Contains("UserName")).Any())
                username = connectionInfos.Where(x => x.Split('=')[0].Contains("userName") || x.Split('=')[0].Contains("UserName")).FirstOrDefault().Split('=')[1].Replace(" ", "");

            if (connectionInfos.Where(x => x.Split('=')[0].Contains("password") || x.Split('=')[0].Contains("Password")).Any())
                password = connectionInfos.Where(x => x.Split('=')[0].Contains("password") || x.Split('=')[0].Contains("Password")).FirstOrDefault().Split('=')[1].Replace(" ", "");

            if (connectionInfos.Where(x => x.Split('=')[0].Contains("database") || x.Split('=')[0].Contains("Database")).Any())
                database = connectionInfos.Where(x => x.Split('=')[0].Contains("database") || x.Split('=')[0].Contains("Database")).FirstOrDefault().Split('=')[1].Replace(" ", "");

            if (server.ToLower().Contains("dynamics.com"))
            {
                sqlConnectionType = ESqlConnectionType.adwithpasswordType;
            }

            if (!string.IsNullOrEmpty(server) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(database))
            {
                sqlInfo = new SqlInfo
                {
                    Server = server,
                    Database = database,
                    UserName = username,
                    Password = password,
                    ESqlConnectionType = sqlConnectionType,
                };
            }
            else
            {
                throw new System.Exception("Invalid ConnectionString");
            }
        }

        public Sql.IOnPremiseSqlService GetOnPremiseSqlService()
        {
            if (!Container.HasKey(typeof(Sql.IOnPremiseSqlService)))
                Container.Register<Sql.IOnPremiseSqlService, Sql.OnPremiseSqlService>(sqlInfo);

            Sql.IOnPremiseSqlService onPremiseEntityService = (Sql.IOnPremiseSqlService)Container.GetInstance(typeof(Sql.IOnPremiseSqlService), sqlInfo);
            if (sqlInfo.ESqlConnectionType == ESqlConnectionType.defaultType)
                return onPremiseEntityService;

            else throw new System.Exception("Service and crm type do not match");
        }

        public Sql.IOnlineSqlService GetOnlineSqlService()
        {
            if (!Container.HasKey(typeof(Sql.IOnlineSqlService)))
                Container.Register<Sql.IOnlineSqlService, Sql.OnlineSqlService>(sqlInfo);

            Sql.IOnlineSqlService onlineEntityService = (Sql.IOnlineSqlService)Container.GetInstance(typeof(Sql.IOnlineSqlService), sqlInfo);
            if (sqlInfo.ESqlConnectionType == ESqlConnectionType.adwithpasswordType)
                return onlineEntityService;

            else throw new System.Exception("Service and crm type do not match");
        }
    }
}

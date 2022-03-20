using System;
using System.Collections.Generic;
using System.Text;

namespace D365.Data.Helper.CRM
{

    public class SqlInfo
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ESqlConnectionType ESqlConnectionType { get; set; }

        public string getConnectionString()
        {
            string cnn = string.Empty;
            ////Data Source=<ip>;Initial Catalog=<db>;User Id=<username>;Password=<password>;
            //Server=demo.database.windows.net; Authentication=Active Directory Password; Database=testdb; User Id=user@domain.com; Password=***
            if (ESqlConnectionType == ESqlConnectionType.defaultType)
            {
                return cnn = $@"Data Source={Server};Initial Catalog={Database};User Id={UserName};Password={Password};";
            }
            else if (ESqlConnectionType == ESqlConnectionType.adwithpasswordType)
            {
                return cnn = $@"Server={Server}; Authentication=Active Directory Password; Database={Database}; User Id={UserName}; Password={Password};";
            }

            throw new Exception("Invalid SqlInfo");
        }
    }
}

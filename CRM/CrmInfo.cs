using System;
using System.Collections.Generic;
using System.Text;

namespace D365.Data.Helper.CRM
{
    public class CrmInfo
    {
        public string ApiUrl { get; set; }

        public string Resource { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public ECrmType CrmType { get; set; }

        public string  TenantId { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}

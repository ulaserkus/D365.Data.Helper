using D365.Data.Helper.CRM;

namespace D365.Data.Helper.Sql
{
    public class OnPremiseSqlService : SqlService<CrmObjects.Entity>, IOnPremiseSqlService
    {
        public OnPremiseSqlService(SqlInfo _SqlInfo) : base(_SqlInfo)
        {

        }

        
    
    }

}

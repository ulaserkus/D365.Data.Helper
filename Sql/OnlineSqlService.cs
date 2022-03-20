using D365.Data.Helper.CRM;
using D365.Data.Helper.CrmObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace D365.Data.Helper.Sql
{
    public class OnlineSqlService : SqlService<Entity>, IOnlineSqlService
    {
        public OnlineSqlService(SqlInfo _SqlInfo) : base(_SqlInfo)
        {

        }

        public override Task UpdateEntityAsync(Entity entity)
        {
            throw new Exception("Online version not support");
        }

        public override Task CreateEntityAsync(Entity entity)
        {
            throw new Exception("Online version not support");
        }

        public override Task DeleteEntityAsync(Entity entity)
        {
            throw new Exception("Online version not support");
        }

        public async override Task<object> ExecuteQueryAsync(string query)
        {
            try
            {
                string cnnStr = _SqlInfo.getConnectionString();

                if (query.ToLower().Contains("update") || query.ToLower().Contains("delete") || query.ToLower().Contains("insert"))
                {
                    throw new Exception("Online version not support");
                }
                else
                {
                    List<Entity> entities = new List<Entity>();
                    using (SqlConnection cnn = new SqlConnection(cnnStr))
                    {
                        await cnn.OpenAsync();
                        SqlCommand cmd = new SqlCommand(query, cnn);
                        DataTable dt = new DataTable();
                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        dt.Load(reader);

                        if (dt.Rows.Count > 0)
                        {
                            string table = query.ToLower().Substring(query.ToLower().IndexOf("from")).Split(' ')[0];
                            DataColumnCollection Columns = dt.Columns;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Entity entity = new Entity(table);
                                DataRow row = dt.Rows[i];
                                for (int y = 0; y < Columns.Count; y++)
                                {
                                    var value = row[Columns[y]];

                                    entity[Columns[y].ToString()] = value;

                                }

                                entities.Add(entity);
                            }
                        }

                    }

                    return entities;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

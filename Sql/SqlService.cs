using D365.Data.Helper.CRM;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using D365.Data.Helper.CrmObjects;
using System.Linq;
using System;

namespace D365.Data.Helper.Sql
{
    public class SqlService<TEntity> : ISqlService<TEntity> where TEntity : Entity
    {
        internal readonly SqlInfo _SqlInfo;

        public SqlService(SqlInfo SqlInfo)
        {
            _SqlInfo = SqlInfo;
        }

        public async virtual Task CreateEntityAsync(TEntity entity)
        {
            try
            {
                entity["createdon"] = DateTime.Now;
                string cnnStr = _SqlInfo.getConnectionString();
                string attText = string.Empty;
                string attValText = string.Empty;
                foreach (var att in entity.Attributes)
                {
                    attText += att.Key;

                    if (att.Value.GetType() != typeof(int) && att.Value.GetType() != typeof(decimal) && att.Value.GetType() != typeof(double))
                        attValText += "'" + att.Value + "'";
                    else
                        attValText += att.Value;

                    if (att.Key != entity.Attributes.Last().Key)
                    {
                        attText += ",";
                        attValText += ",";
                    }
                }

                string columnText = $"({attText})";
                string valText = $"({attValText})";
                string query = $"insert into {entity.LogicalName} " + columnText + " values " + valText;


                using (SqlConnection cnn = new SqlConnection(cnnStr))
                {
                    await cnn.OpenAsync();
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    var res = await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public async virtual Task DeleteEntityAsync(TEntity entity)
        {
            string cnnStr = _SqlInfo.getConnectionString();
            try
            {
                KeyValuePair<string, object> keyValuePair = entity.Attributes.Where(x => (Guid)x.Value == entity.Id).FirstOrDefault();

                string query = $"delete from {entity.LogicalName} where {keyValuePair.Key} = '{keyValuePair.Value}'";

                using (SqlConnection cnn = new SqlConnection(cnnStr))
                {
                    await cnn.OpenAsync();
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    var res = await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        public async virtual Task<object> ExecuteQueryAsync(string query)
        {
            try
            {
                string cnnStr = _SqlInfo.getConnectionString();

                if (query.ToLower().Contains("update") || query.ToLower().Contains("delete") || query.ToLower().Contains("ınsert") || query.ToLower().Contains("insert"))
                {
                    using (SqlConnection cnn = new SqlConnection(cnnStr))
                    {
                        await cnn.OpenAsync();
                        SqlCommand cmd = new SqlCommand(query, cnn);
                        var res = await cmd.ExecuteNonQueryAsync();

                        return null;
                    }
                }
                else
                {
                    List<TEntity> entities = new List<TEntity>();
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

                                entities.Add((TEntity)entity);
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

        public async Task<List<TEntity>> GetAllEntitiesAsync(string entityName)
        {
            try
            {
                List<TEntity> entities = new List<TEntity>();

                string cnnStr = _SqlInfo.getConnectionString();

                using (SqlConnection cnn = new SqlConnection(cnnStr))
                {
                    await cnn.OpenAsync();
                    SqlCommand cmd = new SqlCommand(@"select * from " + entityName, cnn);
                    DataTable dt = new DataTable();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    dt.Load(reader);
                    if (dt.Rows.Count > 0)
                    {
                        DataColumnCollection Columns = dt.Columns;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Entity entity = new Entity(entityName);
                            DataRow row = dt.Rows[i];
                            for (int y = 0; y < Columns.Count; y++)
                            {
                                var value = row[Columns[y]];

                                entity[Columns[y].ToString()] = value;

                            }

                            entities.Add((TEntity)entity);
                        }
                    }

                }

                return entities;

            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<TEntity>> GetAllEntitiesWithColumnsAsync(string entityName, string[] columns)
        {
            try
            {
                List<TEntity> entities = new List<TEntity>();

                string cnnStr = _SqlInfo.getConnectionString();

                using (SqlConnection cnn = new SqlConnection(cnnStr))
                {
                    await cnn.OpenAsync();
                    string selectText = "select ";
                    for (int i = 0; i < columns.Length; i++)
                    {
                        selectText += columns[i];
                        if (i != columns.Length - 1) selectText += ",";
                    }
                    SqlCommand cmd = new SqlCommand(selectText + " from " + entityName, cnn);
                    DataTable dt = new DataTable();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    dt.Load(reader);
                    if (dt.Rows.Count > 0)
                    {
                        DataColumnCollection Columns = dt.Columns;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Entity entity = new Entity(entityName);
                            DataRow row = dt.Rows[i];
                            for (int y = 0; y < Columns.Count; y++)
                            {
                                var value = row[Columns[y]];

                                entity[Columns[y].ToString()] = value;

                            }

                            entities.Add((TEntity)entity);
                        }
                    }

                }

                return entities;

            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        public async Task<TEntity> GetEntityByIdAsync(Guid entityId, string entityName)
        {
            try
            {
                List<TEntity> entities = new List<TEntity>();

                string cnnStr = _SqlInfo.getConnectionString();

                using (SqlConnection cnn = new SqlConnection(cnnStr))
                {
                    await cnn.OpenAsync();
                    SqlCommand cmd = new SqlCommand(@"select * from " + entityName + $" where {entityName}id = '{entityId}'", cnn);
                    DataTable dt = new DataTable();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    dt.Load(reader);
                    if (dt.Rows.Count > 0)
                    {
                        DataColumnCollection Columns = dt.Columns;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Entity entity = new Entity(entityName);
                            DataRow row = dt.Rows[i];
                            for (int y = 0; y < Columns.Count; y++)
                            {
                                var value = row[Columns[y]];

                                entity[Columns[y].ToString()] = value;

                            }

                            entities.Add((TEntity)entity);
                        }
                    }

                }

                return entities.FirstOrDefault();

            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public async virtual Task UpdateEntityAsync(TEntity entity)
        {
            string cnnStr = _SqlInfo.getConnectionString();
            try
            {
                KeyValuePair<string, object> keyValuePair = entity.Attributes.Where(x => (Guid)x.Value == entity.Id).FirstOrDefault();
                string attValText = string.Empty;
                foreach (var att in entity.Attributes)
                {
                    if (att.Value.GetType() != typeof(int) && att.Value.GetType() != typeof(decimal) && att.Value.GetType() != typeof(double))
                        attValText += att.Key + "=" + "'" + att.Value + "'";
                    else
                        attValText += att.Key + "=" + att.Value;

                    if (att.Key != entity.Attributes.Last().Key)
                    {
                        attValText += ",";
                    }
                }
                string valText = $"({attValText})";
                string query = $"update {entity.LogicalName} " + valText + $"where { keyValuePair.Key} = '{keyValuePair.Value}'";

                using (SqlConnection cnn = new SqlConnection(cnnStr))
                {
                    await cnn.OpenAsync();
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    var res = await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
    }
}

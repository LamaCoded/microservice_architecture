using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace orderservices.data.dbContext
{
    public class DatabaseContext
    {
        private readonly IConfiguration _config;
        private string ConnectionString { get; set; }
        public bool EnableDbLogger { get; set; }

        public DatabaseContext(IConfiguration config)
        {
            _config = config;
            ConnectionString = _config.GetConnectionString("DefaultConnection");
        }

        public DataTable ExecuteDataTable(string query, IEnumerable<SqlParameter> parameters = null)
        {
            using var cn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(query, cn);
            if (parameters != null)
                cmd.Parameters.AddRange(parameters.ToArray());

            using var adapter = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            try
            {
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public int ExecuteNonQuery(string query, IEnumerable<SqlParameter> parameters = null)
        {
            using var cn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(query, cn);
            if (parameters != null)
                cmd.Parameters.AddRange(parameters.ToArray());

            cn.Open();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return -1;
            }
        }

        public object ExecuteScalar(string query, IEnumerable<SqlParameter> parameters = null)
        {
            using var cn = new SqlConnection(ConnectionString);
            using var cmd = new SqlCommand(query, cn);
            if (parameters != null)
                cmd.Parameters.AddRange(parameters.ToArray());

            cn.Open();
            try
            {
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public bool ExecuteTransaction(List<string> queries, List<List<SqlParameter>> parametersList = null)
        {
            using var cn = new SqlConnection(ConnectionString);
            cn.Open();
            using var transaction = cn.BeginTransaction();
            try
            {
                for (int i = 0; i < queries.Count; i++)
                {
                    using var cmd = new SqlCommand(queries[i], cn, transaction);
                    if (parametersList != null && parametersList.Count > i && parametersList[i] != null)
                        cmd.Parameters.AddRange(parametersList[i].ToArray());
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("Transaction Error: " + ex.Message);
                return false;
            }
        }
    }
}

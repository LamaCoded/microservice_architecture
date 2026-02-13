using System.Data;
using productservices.data.interfaces;
using userservice.data.dbContext;
using Npgsql;

namespace productservices.data.repo
{
    public class productRepo : Icategory, Iproduct
    {
        private readonly DatabaseContext _db;
        public productRepo(DatabaseContext context)
        {
            _db = context;
        }
        public DbResponse SaveCategory(int id, string name)
        {
            string query = @"
                INSERT INTO ""category"" (""Id"", ""Name"")
                VALUES (@id, @name);
            ";

            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("@id", id),
                new NpgsqlParameter("@name", name),
            };

            int rows = _db.ExecuteNonQuery(query, parameters);

            return rows > 0
                ? new DbResponse { Message = "Category saved successfully" }
                : new DbResponse { HasError = true, Message = "Insert failed" };

        }
        public DataTable GetCategories(int? id)
        {
            string query = "SELECT * FROM category";
            var parameters = new List<NpgsqlParameter>();

            if (id.HasValue)
            {
                query += " WHERE Id = @id";
                parameters.Add(new NpgsqlParameter("@id", id.Value));
            }

            return _db.ExecuteDataTable(query, parameters);
        }
        public DbResponse SaveProduct(int categoryId,string name, string description,double price)
        {
            string query = @"
                INSERT INTO ""products"" (""CategoryId"", ""Title"", ""Description"", ""Price"")
                VALUES (@categoryId, @title, @description, @price);
            ";

            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("@categoryId", categoryId),
                new NpgsqlParameter("@title", name),
                new NpgsqlParameter("@description", description),
                new NpgsqlParameter("@price", price),

            };

            int rows = _db.ExecuteNonQuery(query, parameters);

            return rows > 0
                ? new DbResponse { Message = "Category saved successfully" }
                : new DbResponse { HasError = true, Message = "Insert failed" };
        }
        public DataTable GetProduct(int? id)
        {
            string query = "SELECT * FROM products";
            var parameters = new List<NpgsqlParameter>();

            if (id.HasValue)
            {
                query += " WHERE Id = @id";
                parameters.Add(new NpgsqlParameter("@id", id.Value));
            }

            return _db.ExecuteDataTable(query, parameters);

        }

    }
}

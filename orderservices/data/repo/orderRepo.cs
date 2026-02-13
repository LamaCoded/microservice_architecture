using System.Data;
using Microsoft.Data.SqlClient;
using orderservices.data.dbContext;

namespace orderservices.data.repo
{
    public class orderRepo
    {
        private readonly DatabaseContext _db;
        public orderRepo(DatabaseContext dbContext)
        {
            _db = dbContext;
        }

        public DataTable GetOrders(int? id)
        {
            string query = "SELECT * FROM orders";
            var parameters = new List<SqlParameter>();

            if (id.HasValue)
            {
                query += " WHERE Id = @id";
                parameters.Add(new SqlParameter("@id", id.Value));
            }

            return _db.ExecuteDataTable(query, parameters);
        }

        public DbResponse SaveOrders(int userId, int productId, double price, DateTime date)
        {
            string query = @"
                            INSERT INTO orders (UserId,ProductId, Price, CreatedDate)
                            VALUES (@userId,@productId, @price, @date);
                            ";

            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@productId", productId),
        new SqlParameter("@price", price),
        new SqlParameter("@date", date),
        new SqlParameter("@userId",userId)
    };

            int rows = _db.ExecuteNonQuery(query, parameters);

            return rows > 0
                ? new DbResponse { Message = "Order saved successfully" }
                : new DbResponse { HasError = true, Message = "Failed to save order" };
        }


    }
}

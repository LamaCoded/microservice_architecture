using System.Data;

using Npgsql;
using userservice.data.dbContext;
using userservice.data.interfaces;
using userservice.helper;

namespace userservice.data.repo
{
    public class UserRepo : ITokenRepository
    {
        private readonly DatabaseContext _db;
        private readonly AuthHelper _authHelper;

        public UserRepo(DatabaseContext context, AuthHelper authHelper)
        {
            _db = context;
            _authHelper = authHelper;
        }

        public DataTable GetUser(int? id = null)
        {
            string query = "SELECT * FROM users";
            var parameters = new List<NpgsqlParameter>();

            if (id.HasValue)
            {
                query += " WHERE Id = @id";
                parameters.Add(new NpgsqlParameter("@id", id.Value));
            }

            return _db.ExecuteDataTable(query, parameters);
        }

        public DbResponse SaveUser(string username, string password, string displayname)
        {
            string query = @"
                INSERT INTO ""users"" (""UserName"", ""Password"", ""DisplayName"")
                VALUES (@username, @password, @displayname);
            ";

            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("@username", username),
                new NpgsqlParameter("@password", password), 
                new NpgsqlParameter("@displayname", displayname)
            };

            int rows = _db.ExecuteNonQuery(query, parameters);

            return rows > 0
                ? new DbResponse { Message = "User saved successfully" }
                : new DbResponse { HasError = true, Message = "Insert failed" };
        }

        public DbResponse Login(string username, string password)
        {
            string query = @"SELECT * FROM ""users"" WHERE ""UserName"" = @username AND ""Password"" = @password";
            var parameters = new List<NpgsqlParameter>
            {
                new NpgsqlParameter("@username", username),
                new NpgsqlParameter("@password", password)
            };

            var dt = _db.ExecuteDataTable(query, parameters);

            if (dt.Rows.Count == 0)
            {
                return new DbResponse { HasError = true, Message = "Invalid username or password" };
            }

           
            var userId = dt.Rows[0]["id"].ToString();
            var token = _authHelper.GenerateJwtToken(userId, username);

            return new DbResponse
            {
                Message = "Login successful",
                Response = token
            };
        }
    }
}

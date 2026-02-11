using System.Data;
using Npgsql;
using userservice.data.dbContext;
using userservice.data.interfaces;

namespace userservice.data.repo
{
    public class UserRepo : ITokenRepository
    {
        private readonly DatabaseContext _db;

        public UserRepo(DatabaseContext context)
        {
            _db = context;
        }

        public DataTable GetUser()
        {
            string query = "SELECT * FROM users";
            return _db.ExecuteDataTable(query);
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

    }
}

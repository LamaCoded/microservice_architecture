using System.Data;

namespace userservice.data.interfaces
{
    public interface ITokenRepository
    {
        DataTable GetUser(int? id);
        DbResponse SaveUser(String username, String password, String displayName);
        DbResponse Login(String username, String password);
    }
}

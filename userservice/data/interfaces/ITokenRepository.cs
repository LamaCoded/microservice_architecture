using System.Data;

namespace userservice.data.interfaces
{
    public interface ITokenRepository
    {
        DataTable GetUser();
        DbResponse SaveUser(String username, String password, String displayName);
    }
}

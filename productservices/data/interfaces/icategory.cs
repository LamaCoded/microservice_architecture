using System.Data;

namespace productservices.data.interfaces
{
    public interface Icategory
    {
        DataTable GetCategories(int? id);
        DbResponse SaveCategory(int id, String name);
    }
}

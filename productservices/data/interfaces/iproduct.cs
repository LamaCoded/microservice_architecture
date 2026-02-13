using System.Data;

namespace productservices.data.interfaces
{
    public interface Iproduct
    {
        DataTable GetProduct(int? id);
        DbResponse SaveProduct(int categoryId, string title, string description, double price);
    }
}

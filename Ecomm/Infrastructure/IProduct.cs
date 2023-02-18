using Ecomm.Models;

namespace Ecomm.Infrastructure
{
    public interface IProduct
    {
        IQueryable<Product> GetAll();
        Product GetProductById(int id);
        void Insert(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}

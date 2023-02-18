using Ecomm.Data;
using Ecomm.Models;

namespace Ecomm.Infrastructure
{
    public class ProductRepo : IProduct
    {
        private readonly Context _db;

        public ProductRepo(Context db)
        {
            _db = db;
        }

        public void Delete(Product product)
        {
            _db.Products.Remove(product);
            _db.SaveChanges();
        }

        public IQueryable<Product> GetAll()
        {
            return _db.Products;
        }

        public Product GetProductById(int id)
        {
            return _db.Products.FirstOrDefault(x=>x.Id==id);
        }

        public void Insert(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
        }

        public void Update(Product product)
        {
            _db.Products.Update(product);
            _db.SaveChanges();
        }
    }
}

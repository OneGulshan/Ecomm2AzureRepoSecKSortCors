using Microsoft.AspNetCore.Mvc;
using Ecomm.Models;
using Ecomm.Infrastructure;
using Ecomm.filters;
using Microsoft.AspNetCore.Authorization;

//This Controller is Auto Generated using Scaffolding
namespace Ecomm.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProduct _pdt;

        public ProductsController(IProduct pdt)
        {
            _pdt = pdt;
        }

        // GET: api/Products
        [HttpGet]
        public IQueryable<Product> GetProducts(string sortPrice)
        {
            IQueryable<Product> products;
            switch (sortPrice)
            {
                case "desc":
                    products = (IQueryable<Product>)_pdt.GetAll() // All product is converted here in IQueryable
                        .OrderByDescending(x=>x.Price);
                    break;
                case "asc":
                    products = (IQueryable<Product>)_pdt.GetAll() // All product is converted here in IQueryable
                        .OrderBy(x => x.Price);
                    break;
                default:
                    products = (IQueryable<Product>)_pdt.GetAll();
                    break;
            }
            return products;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _pdt.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            var productbyId = _pdt.GetProductById(id);
            _pdt.Update(productbyId);
            
            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult PostProduct(Product product)
        {
            _pdt.Insert(product);            

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _pdt.GetProductById(id);
            _pdt.Delete(product);
            return Ok();
        }
    }
}

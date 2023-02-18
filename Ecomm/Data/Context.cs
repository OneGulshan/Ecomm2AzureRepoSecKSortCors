using Ecomm.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecomm.Data
{
    public class Context : IdentityDbContext<AppUser> /*DbContext*/ // Identity Used here
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }        
        public DbSet<Book> Books { get; set; }        
        public DbSet<BookCover> BookCovers { get; set; }
        public DbSet<BookWritter> BookWritters { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}

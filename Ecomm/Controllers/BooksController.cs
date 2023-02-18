using Ecomm.Data;
using Ecomm.filters;
using Ecomm.Helper;
using Ecomm.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecomm.Controllers
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly Context _db;
        public BooksController(Context db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Book book)
        {
            book.ImageUrl = await FileHelper.UploadImage(book.ImageFile);
            book.BookUrl = await FileHelper.UploadImage(book.BookFile);
            await _db.Books.AddAsync(book);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;
            var books = await (from book in _db.Books
                                 select new
                                 {
                                     Id = book.Id,
                                     Title = book.Title,
                                     Description = book.Description,
                                     ImageUrl = book.ImageUrl,
                                     BookUrl = book.BookUrl
                                 }).ToListAsync();
            return Ok(books.Skip((currentPageNumber-1)*currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> BookDetails(int id)
        {
            var book = await _db.Books.Where(x=>x.Id==id).FirstOrDefaultAsync();
            return Ok(book);
        }
        //api/books/treandingbooks
        [HttpGet("[action]")]
        public async Task<IActionResult> TreandingBooks()
        {
            var books = await (from book in _db.Books
                               where book.Trending==true
                               select new
                               {
                                   Id = book.Id,
                                   Title = book.Title,
                                   Description = book.Description,
                                   ImageUrl = book.ImageUrl,
                                   BookUrl = book.BookUrl
                               }).ToListAsync();
            return Ok(books);
        }        
        [HttpGet("[action]")]
        public async Task<IActionResult> NewBooks()
        {
            var books = await (from book in _db.Books
                               orderby book.CreatedDate descending
                               select new
                               {
                                   Id = book.Id,
                                   Title = book.Title,
                                   Description = book.Description,
                                   ImageUrl = book.ImageUrl,
                                   BookUrl = book.BookUrl
                               }).Take(5).ToListAsync();
            return Ok(books);
        }        
        [HttpGet("[action]")]
        public async Task<IActionResult> SearchBook(string query)
        {
            var books = await (from book in _db.Books
                               where book.Title.StartsWith(query)
                               select new
                               {
                                   Id = book.Id,
                                   Title = book.Title,
                                   Description = book.Description,
                                   ImageUrl = book.ImageUrl,
                                   BookUrl = book.BookUrl
                               }).Take(5).ToListAsync();
            return Ok(books);
        }
    }
}

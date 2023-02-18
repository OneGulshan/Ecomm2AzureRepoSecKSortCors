using Ecomm.Data;
using Ecomm.Helper;
using Ecomm.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecomm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoversController : ControllerBase
    {
        private readonly Context _db;
        public CoversController(Context db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] BookCover cover)
        {
            cover.ImageUrl = await FileHelper.UploadImage(cover.ImageFile);
            await _db.BookCovers.AddAsync(cover);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetCovers(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;
            var covers = await (from cover in _db.BookCovers
                                 select new
                                 {
                                     Id = cover.Id,
                                     Title = cover.Title,
                                     ImageUrl = cover.ImageUrl,
                                     WrriterId = cover.BookWritterId
                                 }).ToListAsync();
            return Ok(covers.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CoverDetails(int id)
        {
            var cover = await _db.BookCovers.Include(x => x.Books).
                Where(x => x.Id == id).FirstOrDefaultAsync();
            return Ok(cover);
        }
    }   
}

using Ecomm.Data;
using Ecomm.Helper;
using Ecomm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ecomm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WritersController : ControllerBase
    {
        private readonly Context _db;
        public WritersController(Context db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] BookWritter writer)
        {
            writer.ImageUrl = await FileHelper.UploadImage(writer.ImageFile);
            await _db.BookWritters.AddAsync(writer);
            await _db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetWriters(int? pageNumber, int? pageSize)
        {
            //var writers = await _db.BookWritters.ToListAsync();
            //return Ok(writers);
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;
            var writers = await (from writer in _db.BookWritters // here we return only selected values only this is the benefit here
                                 select new
                                 {
                                     Id = writer.Id, // Id <- ye anonymous name hote hai hamare pass kuch bhi rakh sakte ham
                                     Name = writer.Name,
                                     ImageUrl = writer.ImageUrl

                                 }).ToListAsync();
            return Ok(writers.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> WriterDetails(int id)
        {
            var writer = await _db.BookWritters.Include(x => x.Books). // yahan 1 writer with its books aa jaegi
                Where(x => x.Id == id).FirstOrDefaultAsync();
            return Ok(writer);
        }
    }
}

//BookApi Configure
//now we host webapi on azure, download getpublishedapi now<-taki website info gotit from this file bec in vs we have a option we can directly select this file
//In Notification Pin to dashboard -> Create new -> Dashboard name -> create and pin -> pinned to dashboard -> open dashboard -> download get publish profile <- get publish profile(by using this profile file we can host our webapi easily)
//Create a Sql database

//Create New Sql Database in azure steps -> click on sql database -> resource group (select your BookApi Configure resource here) -> database name (your coustom database name write here) -> Server create new -> server name -> server admin login details(Admin@123) -> Ok -> Review+create -> Create <- pick constr from this db for coping constr in appsetting.
//On firewall settings of this db otherwise you can't access this db in your webapi -> pin this db also in dashboard


//what is the meaning of custom domain
//A custom domain is a unique branded name that identifies a website. For example, NationBuilder's custom domain is nationbuilder.com. Custom domains, also known as vanity URLs, appear in the address bar at the top of every browser.
//Dekhiye itna cheezon ko complicated mat banaiye jisse aap chijon ko sikh hi na saken
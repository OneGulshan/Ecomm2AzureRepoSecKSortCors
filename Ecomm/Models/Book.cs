using System.ComponentModel.DataAnnotations.Schema;

namespace Ecomm.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string BookUrl { get; set; }
        public bool Trending { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string ISBNNUMBER { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public IFormFile BookFile { get; set; }
        public int? BookCoverId { get; set; }        
        public int BookWritterId { get; set; }        
    }
}
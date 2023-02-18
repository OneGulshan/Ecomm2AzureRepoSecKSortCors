using System.ComponentModel.DataAnnotations.Schema;

namespace Ecomm.Models
{
    public class BookCover
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int BookWritterId { get; set; }
        public BookWritter BookWritter { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}

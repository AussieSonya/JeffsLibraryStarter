using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models.Books
{
    public class GetBooksResponseItem
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
    }
}

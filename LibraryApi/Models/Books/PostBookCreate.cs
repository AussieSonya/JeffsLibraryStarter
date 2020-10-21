using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models.Books
{
    public class PostBookCreate : IValidatableObject
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        [MaxLength(200)]
        public string Author { get; set; }
        [Range(1, 5000)]
        public int NumberOfPages { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Author.ToLower() == "king" && NumberOfPages > 300)
            {
                yield return new ValidationResult("You won't really read it.", new string[] {
                    nameof(Author), nameof(NumberOfPages) });
            }
        }
    }

}

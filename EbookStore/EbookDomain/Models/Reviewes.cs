using System.ComponentModel.DataAnnotations;

namespace EbookDomain.Models
{
    public class Reviewes
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Review Text cannot be greater than 150")]
        [MinLength(4, ErrorMessage = "Review Text cannot be less than 4")]
        public string ReviewText { get; set; }

        [Required]
        public int Rating { get; set; }

        public int BookId { get; set; }

        public Book book { get; set; }

    }
}

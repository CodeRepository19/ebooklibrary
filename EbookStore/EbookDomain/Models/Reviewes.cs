using System.ComponentModel.DataAnnotations;

namespace EbookDomain.Models
{
    public class Reviewes
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "ReviewText should be maximum of 50 characters")]
        [MinLength(4, ErrorMessage = "ReviewText should be minimum of 4 characters")]
        public string ReviewText { get; set; }

        [Required]
        public int Rating { get; set; }

        public int BookId { get; set; }

        public Book book { get; set; }

    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EbookDomain.Models
{
    public class Technology
    {
        [Key]
        public int TechnologyId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Technology Name cannot be greater than 50")]
        [MinLength(4, ErrorMessage = "Technology Name cannot be less than 4")]
        public string TechnologyName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Technology Domain Name cannot be greater than 50")]
        [MinLength(4, ErrorMessage = "Technology Domain Name cannot be less than 4")]
        public string TechnologyDomain { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
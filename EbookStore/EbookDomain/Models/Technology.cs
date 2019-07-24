using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EbookDomain.Models
{
    public class Technology
    {
        [Key]
        public int TechnologyId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Technology name should be maximum of 50 characters")]
        [MinLength(4, ErrorMessage = "Technology name should be minimum of 4 characters")]
        public string TechnologyName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Technology domain should be maximum of 50 characters")]
        [MinLength(4, ErrorMessage = "Technology domain should be minimum of 4 characters")]
        public string TechnologyDomain { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
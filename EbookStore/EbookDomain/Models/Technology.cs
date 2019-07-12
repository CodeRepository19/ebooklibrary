using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EbookDomain.Models
{
    public class Technology
    {
        [Key]
        public int TechnologyId { get; set; }

        [Required]
        [StringLength(100)]
        public string TechnologyName { get; set; }

        [Required]
        [StringLength(100)]
        public string TechnologyDomain { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}

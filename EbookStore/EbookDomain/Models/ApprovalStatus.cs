using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EbookDomain.Models
{
  public  class ApprovalStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; }       

        public ICollection<Book> Books { get; set; }
    }
}

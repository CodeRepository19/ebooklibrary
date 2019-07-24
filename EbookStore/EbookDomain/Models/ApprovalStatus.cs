using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EbookDomain.Models
{
  public  class ApprovalStatus
    {
        [Key]
        public int StatusId { get; set; }

        [Required, MaxLength(20, ErrorMessage = "Status should be maximum of 20 characters")]
        public string Status { get; set; }       

        public ICollection<Book> Books { get; set; }
    }
}

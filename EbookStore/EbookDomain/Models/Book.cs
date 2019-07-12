using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace EbookDomain.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Book Name cannot be greater than 50")]
        [MinLength(4, ErrorMessage = "Book Name cannot be less than 4")]
        [Display(Name = "Book Name")]
        public string BookName { get; set; }

        [Required]
        [MaxLength(150, ErrorMessage = "Description cannot be greater than 50")]
        [MinLength(10, ErrorMessage = "Description cannot be less than 10")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [BindRequired]
        [Required(ErrorMessage = "Select a Technology")]
        [Display(Name = "Technology")]
        public int TechnologyId { get; set; }

        public Technology technology { get; set; }

    }
}

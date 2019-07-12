using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EbookDomain.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Book Name")]
        public string BookName { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [Display(Name = "Technology")]
        public int TechnologyId { get; set; }

        public Technology technology { get; set; }

    }
}

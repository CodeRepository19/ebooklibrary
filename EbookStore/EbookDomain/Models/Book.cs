using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace EbookDomain.Models
{
    /// <summary>
    ///  This is the Model class Here we create properties these will worked as column names in Database
    ///  In our case our class Name "Book" is the Table Name in Database
    ///  The Properties linke "BookId","BookName","Description","ImageUrl","TechnologyId" are column names 
    /// </summary>
    public class Book
    {
        /// <summary>
        /// The Below line contains "[Key]" which is called DataAnnotation to make the column "BookId" as Primary Key
        /// </summary>
        [Key]
        public int BookId { get; set; }

        /// <summary>
        /// The Below line contains "[Required]" which is called DataAnnotation used to Validate the column "BookName" Input data in Presentaion Layer
        /// </summary>
        [Required]
        [MaxLength(50, ErrorMessage = "Book Name cannot be greater than 50")]
        [MinLength(4, ErrorMessage = "Book Name cannot be less than 4")]
        // This will make Display the Column Name "Book Name"  instead of actual Column "BookName"
        [Display(Name = "Book Name")]
        public string BookName { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Description cannot be greater than 500")]
        [MinLength(10, ErrorMessage = "Description cannot be less than 10")]
        // This will make Display the Column Name "Description"  instead of actual Column "Description"
        [Display(Name = "Description")]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        /// <summary>
        /// The Below line contains "[BindRequired]" which is called DataAnnotation used to Validate the column "TechnologyId" DropDown data in Presentaion Layer
        /// </summary>
        [BindRequired]
        [Required(ErrorMessage = "Select a Technology")]
        // This will make Display the Column Name "Technology"  instead of actual Column "TechnologyId"
        [Display(Name = "Technology")]
        public int TechnologyId { get; set; }

        public Technology technology { get; set; }

    }
}

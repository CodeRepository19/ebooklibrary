using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
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
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]
        [MaxLength(50, ErrorMessage = "Book Name Should be maximum of 50 Characters")]
        [MinLength(4, ErrorMessage = "Book Name Should be minimum of 4 Characters")]
        // This will make Display the Column Name "Book Name"  instead of actual Column "BookName"
        [Display(Name = "Book Name")]
        public string BookName { get; set; }

        [Required]
        [MaxLength(5000, ErrorMessage = "Description Should be maximum of 5000 Characters")]
        [MinLength(10, ErrorMessage = "Description Should be minimum of 10 Characters")]
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
        public ICollection<Reviewes> reviewes { get; set; }

        public string CreatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; }

        public string ApprovedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ApprovedDate { get; set; }

        public int StatusId { get; set; }

        public ApprovalStatus approvalStatus { get; set; }

        [MaxLength(500, ErrorMessage = "Remarks Should be minimum of 10 Characters")]
        public string Remarks { get; set; }

        [Required]
        public string Author { get; set; }

        [Required, DataType(DataType.Date)]
        public string PublishedDate { get; set; }

    }
}
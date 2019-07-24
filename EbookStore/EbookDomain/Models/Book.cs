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
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9\d .]*$", ErrorMessage = "Sorry, only letters(a-z), number(0-9), space and period(.) are allowed")]
        [MaxLength(50, ErrorMessage = "Book Name should be maximum of 50 characters")]
        [MinLength(4, ErrorMessage = "Book Name should be minimum of 4 characters")]
        // This will make Display the Column Name "Book Name"  instead of actual Column "BookName"
        [Display(Name = "Book Name")]
        public string BookName { get; set; }

        [Required]
        [MaxLength(5000, ErrorMessage = "Description should be maximum of 5000 characters")]
        [MinLength(10, ErrorMessage = "Description should be minimum of 10 characters")]
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

        [MaxLength(500, ErrorMessage = "Description should be maximum of 500 characters")]
        [MinLength(10, ErrorMessage = "Description should be minimum of 10 characters")]
        public string Remarks { get; set; }

        [RegularExpression(@"^[a-zA-Z][a-zA-Z .]*$", ErrorMessage = "Sorry, only letters(a-z), space and period(.) are allowed")]
        [Required]
        public string Author { get; set; }

        [Required, DataType(DataType.Date)]
        [RegularExpression(@"((18|19|20)[0-9]{2}[\-.](0[13578]|1[02])[\-.](0[1-9]|[12][0-9]|3[01]))|(18|19|20)[0-9]{2}[\-.](0[469]|11)[\-.](0[1-9]|[12][0-9]|30)|(18|19|20)[0-9]{2}[\-.](02)[\-.](0[1-9]|1[0-9]|2[0-8])|(((18|19|20)(04|08|[2468][048]|[13579][26]))|2000)[\-.](02)[\-.]29", ErrorMessage = "Please enter or select valid date")]
        //[Range(typeof(DateTime), "1/1/1900", "12/31/9999")]
        [Display(Name = "Published Date")]
        public string PublishedDate { get; set; }

    }
}
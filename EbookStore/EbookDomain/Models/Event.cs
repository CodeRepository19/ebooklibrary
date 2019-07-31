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
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        /// <summary>
        /// The Below line contains "[Required]" which is called DataAnnotation used to Validate the column "EventName" Input data in Presentaion Layer
        /// </summary>
        [Required]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9\d .]*$", ErrorMessage = "Sorry, only letters(a-z), number(0-9), space and period(.) are allowed")]
        [MaxLength(50, ErrorMessage = "Event Name should be maximum of 50 characters")]
        [MinLength(4, ErrorMessage = "Event Name should be minimum of 4 characters")]
        // This will make Display the Column Name "Book Name"  instead of actual Column "BookName"
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        public string EventDate { get; set; }

        [Required]
        [MaxLength(5000, ErrorMessage = "Event Description should be maximum of 5000 characters")]
        [MinLength(10, ErrorMessage = "Event Description should be minimum of 10 characters")]
        // This will make Display the Column Name "Description"  instead of actual Column "Description"
        [Display(Name = "Event Description")]
        public string EventDescription { get; set; }

        public string CreatedBy { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; }
    }
}
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EbookUI.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [RegularExpression(@"\s[0-1]{1}[0-9]{0,2}", ErrorMessage = "Sorry, only numbers are allowed")]
        public string Age { get; set; }
       
    }
}

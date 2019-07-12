using System;
using System.Collections.Generic;
using System.Text;
using EbookDomain.Models;
using Microsoft.AspNetCore.Http;

namespace EbookApplication.ViewModels
{
    public class BookViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public Book book { get; set; }
        public IFormFile Image { get; set; }
    }
}

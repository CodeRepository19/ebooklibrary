﻿using EbookDomain.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EbookApplication.ViewModels
{
    public class BookViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Technology> Technologys { get; set; }
        public Book book { get; set; }
        public Technology technology { get; set; }
        public IFormFile Image { get; set; }
        public string ExistingImageUrl { get; set; }
    }
}
using EbookDomain.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EbookApplication.ViewModels
{
    public class EventViewModel
    {
        public IEnumerable<Event> Events { get; set; }

        public Event events { get; set; }
    }
}

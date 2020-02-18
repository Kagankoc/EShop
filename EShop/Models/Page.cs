using System;
using System.ComponentModel.DataAnnotations;

namespace EShop.Models
{
    public class Page
    {
        public Guid Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum Length is 2")]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Minimum Length is 4")]
        public string Content { get; set; }
        public int Sorting { get; set; }

    }
}

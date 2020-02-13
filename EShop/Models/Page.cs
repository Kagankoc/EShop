using System;
using System.ComponentModel.DataAnnotations;

namespace EShop.Models
{
    public class Page
    {
        public Guid Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum Length is 2")]
        public String Title { get; set; }
        public String Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Minimum Length is 4")]
        public String Content { get; set; }
        public int Sorting { get; set; }

    }
}

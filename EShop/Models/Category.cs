using System;
using System.ComponentModel.DataAnnotations;

namespace EShop.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        [Required, MinLength(2, ErrorMessage = "Minimum Length is 2")]
        [RegularExpression(@"^[a-zA-Z-]+$", ErrorMessage = "Only Letters are Allowed")]
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
    }
}

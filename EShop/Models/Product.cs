using EShop.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum Length is 2")]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required, MinLength(10, ErrorMessage = "Minimum Length is 10")]
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Display(Name = "Category")]
        [IsNotDefault]
        public Guid CategoryId { get; set; }

        public string Image { get; set; }
        public virtual Category Category { get; set; }
        
        [NotMapped]
        [ValidExtension]
        public IFormFile ImageFile { get; set; }


    }
}

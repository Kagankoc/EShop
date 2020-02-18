using System;
using System.ComponentModel.DataAnnotations;

namespace EShop.Infrastructure
{
    public class IsNotDefaultAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var guidValue = value as Guid?;
            return guidValue != Guid.Empty ? ValidationResult.Success : new ValidationResult("Please Choose a Category");
        }
    }
}

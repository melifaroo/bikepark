using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public class ValidateDateRange : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dt = (DateTime)value;

            if (dt >= DateTime.Now.AddHours(-1))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Date is not in given range.");
            }
        }
    }
}

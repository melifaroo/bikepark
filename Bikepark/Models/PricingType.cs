using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public enum PricingType
    {
        [Display(Name = "One-Time")]
        OneTime,
        [Display(Name = "Hourly")]
        Hourly,
        [Display(Name = "Service")]
        Service
    }
}
using System.ComponentModel.DataAnnotations;

namespace Bikepark.Models
{
    public enum PricingType
    {
        [Display(Name = "Единоразовая")]
        OneTime,
        [Display(Name = "Почасовая")]
        Hourly,
        [Display(Name = "Ремонт")]
        Service
    }
}
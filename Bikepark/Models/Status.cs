using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum Status
    {
        [Display(Name = "Draft")]
        Draft,

        [Display(Name = "Scheduled")]
        Scheduled,

        [Display(Name = "Active")]
        Active,

        [Display(Name = "Closed")]
        Closed,

        [Display(Name = "Service required")]
        Service,

        [Display(Name = "On Service")]
        OnService,

        [Display(Name = "Service completed")]
        Fixed,     
    }
}


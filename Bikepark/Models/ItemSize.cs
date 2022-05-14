using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum ItemSize
    {
        [Display(Name = "")]
        NA,
        [Display(Name = "S")]
        S,
        [Display(Name = "M")]
        M,
        [Display(Name = "L")]
        L,
        [Display(Name = "XL")]
        XL,
    }
}


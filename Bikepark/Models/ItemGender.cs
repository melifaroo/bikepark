using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum ItemGender
    {
        [Display(Name = "") ]
        Unisex,

        [Display(Name = "Men's")]
        M,

        [Display(Name = "Women's")]
        W,
    }

}


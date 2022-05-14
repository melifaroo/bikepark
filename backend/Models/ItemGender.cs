using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum ItemGender
    {
        [Display(Name = "") ]
        Unisex,
        [Display(Name = "Ì")]
        M,
        [Display(Name = "Æ")]
        W,
    }

}


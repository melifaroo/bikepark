using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum AttentionStatus
    {
        [Display(Name = "Attention")] 
        Attention,

        [Display(Name = "Warning")] 
        Warning,

        [Display(Name = "")] 
        Current
    }
}


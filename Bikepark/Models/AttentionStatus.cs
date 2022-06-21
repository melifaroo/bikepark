using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum AttentionStatus
    {
        [Display(Name = "Внимание")] 
        Attention,
        [Display(Name = "Предупреждение")] 
        Warning,
        [Display(Name = "Текущий")] 
        Current
    }
}


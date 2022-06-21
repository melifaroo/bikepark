using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum AttentionStatus
    {
        [Display(Name = "��������")] 
        Attention,
        [Display(Name = "��������������")] 
        Warning,
        [Display(Name = "�������")] 
        Current
    }
}


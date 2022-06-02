using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bikepark.Models
{
    public enum Status
    {
        [Display(Name = "�� ��������")]
        Draft,      
        [Display(Name = "������������")]
        Scheduled,   
        [Display(Name = "�����")]
        Active,     
        [Display(Name = "��������")]
        Closed,

        [Display(Name = "�������� �������")]
        Service,
        [Display(Name = "������������ ������")]
        ServiceScheduled,
        [Display(Name = "� �������")]
        OnService,
        [Display(Name = "��������������")]
        Fixed,     
    }
}


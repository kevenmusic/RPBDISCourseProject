using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarriageAgency.DataLayer.Models
{
    public partial class Service
    {
        [Display(Name = "Код услуги")]
        [Key]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Дополнительная услуга обязательна.")]
        [Display(Name = "Код дополнительной услуги")]
        [ForeignKey("AdditionalService")]
        public int AdditionalServiceId { get; set; }

        [Required(ErrorMessage = "Клиент обязателен.")]
        [Display(Name = "Код клиента")]
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Сотрудник обязателен.")]
        [Display(Name = "Код сотрудника")]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Дата обязательна.")]
        [Display(Name = "Дата услуги")]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "Стоимость обязательна.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Стоимость должна быть положительным числом.")]
        [Display(Name = "Стоимость")]
        public decimal Cost { get; set; }

        [Display(Name = "Дополнительная услуга")]
        public virtual AdditionalService AdditionalService { get; set; } = null!;

        [Display(Name = "Клиент")]
        public virtual Client Client { get; set; } = null!;

        [Display(Name = "Сотрудник")]
        public virtual Employee Employee { get; set; } = null!;
    }
}
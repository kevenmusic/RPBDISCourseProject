using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.Models
{
    public partial class Service
    {
        [Display(Name = "Код услуги")]
        public int ServiceId { get; set; }

        [Display(Name = "Код дополнительной услуги")]
        public int AdditionalServiceId { get; set; }

        [Display(Name = "Код клиента")]
        public int ClientId { get; set; }

        [Display(Name = "Код сотрудника")]
        public int EmployeeId { get; set; }

        [Display(Name = "Дата услуги")]
        public DateOnly Date { get; set; }

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
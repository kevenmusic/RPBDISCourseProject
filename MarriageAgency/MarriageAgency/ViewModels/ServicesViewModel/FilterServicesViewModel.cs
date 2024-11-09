using System.ComponentModel.DataAnnotations;
using MarriageAgency.DataLayer.Models;

namespace MarriageAgency.ViewModels.ServicesViewModel
{
    public class FilterServicesViewModel
    {
        [Display(Name = "Код услуги")]
        public int ServiceId { get; set; }

        [Display(Name = "Дата услуги")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Дата услуги обязательна.")]
        public DateOnly Date { get; set; }

        [Display(Name = "Стоимость")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Стоимость должна быть положительным числом.")]
        [Required(ErrorMessage = "Стоимость обязательна.")]
        public decimal Cost { get; set; }
        public decimal? MinCost { get; set; }
        public decimal? MaxCost { get; set; }

        [Display(Name = "Дополнительная услуга")]
        public string AdditionalServiceName { get; set; } = null!;

        [Display(Name = "Клиент")]
        public string ClientName { get; set; } = null!;

        [Display(Name = "Сотрудник")]
        public string EmployeeName { get; set; } = null!;
    }
}

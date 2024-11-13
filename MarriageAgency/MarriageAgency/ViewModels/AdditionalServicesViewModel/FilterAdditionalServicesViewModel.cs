using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.ViewModels.AdditionalServicesViewModel
{
    public class FilterAdditionalServicesViewModel
    {
        [Display(Name = "Название услуги")]
        public string AdditionalServiceName { get; set; } = null!;

        [Display(Name = "Описание услуги")]
        public string? Description { get; set; }

        [Display(Name = "Цена услуги")]
        public decimal Price { get; set; }
    }
}

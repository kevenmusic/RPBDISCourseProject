using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels.ClientsViewModel;
using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.ViewModels.NationalitiesViewModel
{
    public class FilterNationalitiesViewModel
    {
        [Display(Name = "Название национальности")]
        public string NationalityName { get; set; } = null!;

        [Display(Name = "Примечания")]
        public string? Notes { get; set; }
    }
}

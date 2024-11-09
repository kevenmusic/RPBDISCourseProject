using MarriageAgency.DataLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.ViewModels.ClientsViewModel
{
    public class FilterClientsViewModel
    {
        [Display(Name = "Клиент")]
        public string ClientName { get; set; } = null!;

        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }

        [Display(Name = "Пол")]
        public string Gender { get; set; } = null!;

        [Display(Name = "Дата рождения")]
        public DateOnly? BirthDate { get; set; }

        [Display(Name = "Профессия")]
        public string? Profession { get; set; }

        [Display(Name = "Контактные данные")]
        public virtual Contact? Contact { get; set; }

        [Display(Name = "Национальность")]
        public string NationalityName { get; set; }

        [Display(Name = "Фото")]
        public string ClientPhoto { get; set; } = null!;

        [Display(Name = "Возраст")]
        public int? Age { get; set; }

        [Display(Name = "Знак зодиака")]
        public string ZodiacSignName { get; set; }
    }
}

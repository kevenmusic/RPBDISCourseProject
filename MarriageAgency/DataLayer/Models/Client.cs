using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarriageAgency.DataLayer.Models
{
    public partial class Client
    {
        //Id Клиента
        [Display(Name = "Код клиента")]
        public int ClientId { get; set; }

        [Display(Name = "Имя Клиента")]
        [Required(ErrorMessage = "Поле Имя клиента обязательно для заполнения.")]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "Числа не допускаются.")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Поле Фамилия клиента обязательно для заполнения.")]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "Числа не допускаются.")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Отчество")]
        [Required(ErrorMessage = "Поле Отчество клиента обязательно для заполнения.")]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "Числа не допускаются.")]
        public string? MiddleName { get; set; }

        [Display(Name = "Пол")]
        public string Gender { get; set; } = null!;

        [Display(Name = "Дата рождения")]
        public DateOnly? BirthDate { get; set; }

        [Display(Name = "Знак зодиака")]
        public int? ZodiacSignId { get; set; }

        [Display(Name = "Национальность")]
        public int? NationalityId { get; set; }

        [Display(Name = "Профессия")]
        public string? Profession { get; set; }

        [Display(Name = "Контактные данные")]
        public virtual Contact? Contact { get; set; }

        [Display(Name = "Национальность")]
        public virtual Nationality? Nationality { get; set; }

        [Display(Name = "Фото")]
        public string ClientPhoto { get; set; } = null!;

        [Display(Name = "Front Image")]
        [NotMapped]
        public IFormFile FrontImage { get; set; }

        [Display(Name = "Физические данные")]
        public virtual PhysicalAttribute? PhysicalAttribute { get; set; }

        [Display(Name = "Знак зодиака")]
        public virtual ZodiacSign? ZodiacSign { get; set; }

        [Display(Name = "Услуги")]
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
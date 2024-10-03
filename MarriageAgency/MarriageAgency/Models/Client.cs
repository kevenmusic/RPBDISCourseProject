using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.Models
{
    public partial class Client
    {
        //Id Клиента
        [Display(Name = "Код клиента")]
        public int ClientId { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Отчество")]
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
        public virtual Photo? Photo { get; set; }

        [Display(Name = "Физические данные")]
        public virtual PhysicalAttribute? PhysicalAttribute { get; set; }

        [Display(Name = "Знак зодиака")]
        public virtual ZodiacSign? ZodiacSign { get; set; }

        [Display(Name = "Услуги")]
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
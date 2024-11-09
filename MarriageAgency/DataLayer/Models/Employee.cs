using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.DataLayer.Models
{
    public partial class Employee
    {
        [Display(Name = "Код сотрудника")]
        public int EmployeeId { get; set; }

        [Display(Name = "Имя Сотрудника")]
        [Required(ErrorMessage = "Поле Имя сотрудника обязательно для заполнения.")]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "Числа не допускаются.")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Поле Фамилия сотрудника обязательно для заполнения.")]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "Числа не допускаются.")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Отчество")]
        [Required(ErrorMessage = "Поле Отчество сотрудника обязательно для заполнения.")]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "Числа не допускаются.")]
        public string MiddleName { get; set; } = null!;

        [Display(Name = "Должность")]
        [Required(ErrorMessage = "Поле Должность сотрудника обязательно для заполнения.")]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "Числа не допускаются.")]
        public string? Position { get; set; }

        [Display(Name = "Дата рождения")]
        [Required(ErrorMessage = "Поле Фамилия клиента обязательно для заполнения.")]
        public DateOnly? BirthDate { get; set; }

        [Display(Name = "Услуги")]
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
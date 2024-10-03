using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.Models
{
    public partial class Employee
    {
        [Display(Name = "Код сотрудника")]
        public int EmployeeId { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; } = null!;

        [Display(Name = "Должность")]
        public string? Position { get; set; }

        [Display(Name = "Дата рождения")]
        public DateOnly? BirthDate { get; set; }

        [Display(Name = "Услуги")]
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
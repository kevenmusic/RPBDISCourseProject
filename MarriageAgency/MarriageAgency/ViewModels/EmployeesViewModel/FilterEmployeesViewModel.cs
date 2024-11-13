using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.ViewModels.EmployeesViewModel
{
    public class FilterEmployeesViewModel
    {
        [Display(Name = "Имя Сотрудника")]
        public string EmployeeName { get; set; } = null!;

        [Display(Name = "Фамилия")]
        public string EmployeeLastName { get; set; } = null!;

        [Display(Name = "Отчество")]
        public string EmployeeMiddleName { get; set; } = null!;

        [Display(Name = "Должность")]
        public string? Position { get; set; }

        [Display(Name = "Дата рождения")]
        public DateOnly? BirthDate { get; set; }
    }
}

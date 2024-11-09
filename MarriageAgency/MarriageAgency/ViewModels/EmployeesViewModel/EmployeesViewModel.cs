using MarriageAgency.DataLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.ViewModels.EmployeesViewModel
{
    public class EmployeesViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }

        public FilterEmployeesViewModel FilterEmployeesViewModel { get; set; }

        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}

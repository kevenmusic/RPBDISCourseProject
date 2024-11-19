using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels.SortViewModels;

namespace MarriageAgency.ViewModels.EmployeesViewModel
{
    public class EmployeesViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }

        public FilterEmployeesViewModel FilterEmployeesViewModel { get; set; }

        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        // Порядок сортировки
        public EmployeeSortViewModel SortViewModel { get; set; }
    }
}

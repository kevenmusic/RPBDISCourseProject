using MarriageAgency.ViewModels.SortStates;
namespace MarriageAgency.ViewModels.SortViewModels
{
    public class EmployeeSortViewModel
    {
        public EmployeeSortState EmployeeNameSort { get; set; }      // Сортировка по имени сотрудника
        public EmployeeSortState EmployeeLastNameSort { get; set; }  // Сортировка по фамилии сотрудника
        public EmployeeSortState EmployeeMiddleNameSort { get; set; } // Сортировка по отчеству сотрудника
        public EmployeeSortState BirthDateSort { get; set; }         // Сортировка по дате рождения
        public EmployeeSortState CurrentState { get; set; }          // Текущее состояние сортировки
        
        public EmployeeSortViewModel(EmployeeSortState sortOrder)
        {
            // Установка сортировки для имени сотрудника
            EmployeeNameSort = sortOrder == EmployeeSortState.EmployeeNameAsc ? EmployeeSortState.EmployeeNameDesc : EmployeeSortState.EmployeeNameAsc;

            // Установка сортировки для фамилии сотрудника
            EmployeeLastNameSort = sortOrder == EmployeeSortState.EmployeeLastNameAsc ? EmployeeSortState.EmployeeLastNameDesc : EmployeeSortState.EmployeeLastNameAsc;

            // Установка сортировки для отчества сотрудника
            EmployeeMiddleNameSort = sortOrder == EmployeeSortState.EmployeeMiddleNameAsc ? EmployeeSortState.EmployeeMiddleNameDesc : EmployeeSortState.EmployeeMiddleNameAsc;
            // Установка сортировки для даты рождения
            BirthDateSort = sortOrder == EmployeeSortState.BirthDateAsc ? EmployeeSortState.BirthDateDesc : EmployeeSortState.BirthDateAsc;
            // Установка текущего состояния сортировки
            CurrentState = sortOrder;
        }
    }
}

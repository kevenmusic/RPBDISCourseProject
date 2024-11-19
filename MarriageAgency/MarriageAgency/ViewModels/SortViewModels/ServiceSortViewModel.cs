using MarriageAgency.ViewModels.SortStates;

namespace MarriageAgency.ViewModels.SortViewModels
{
    public class ServiceSortViewModel
    {
        public ServiceSortState EmployeeNameSort { get; set; }      // Сортировка по имени сотрудника
        public ServiceSortState ClientNameSort { get; set; }        // Сортировка по имени клиента
        public ServiceSortState AdditionalNameSort { get; set; }    // Сортировка по имени дополнительной услуги
        public ServiceSortState CostSort { get; set; }              // Сортировка по стоимости
        public ServiceSortState CurrentState { get; set; }          // Текущее состояние сортировки

        public ServiceSortViewModel(ServiceSortState sortOrder)
        {
            // Установка сортировки для имени клиента
            ClientNameSort = sortOrder == ServiceSortState.ClientNameAsc ? 
                ServiceSortState.ClientNameDesc : ServiceSortState.ClientNameAsc;
            // Установка сортировки для имени сотрудника
            EmployeeNameSort = sortOrder == ServiceSortState.EmployeeNameAsc ? 
                ServiceSortState.EmployeeNameDesc : ServiceSortState.EmployeeNameAsc;
            // Установка сортировки для имени дополнительной услуги
            AdditionalNameSort = sortOrder == ServiceSortState.AdditionalNameAsc ?
                ServiceSortState.AdditionalNameDesc : ServiceSortState.AdditionalNameAsc;
            // Установка сортировки для стоимости
            CostSort = sortOrder == ServiceSortState.CostAsc ? 
                ServiceSortState.CostDesc : ServiceSortState.CostAsc;
            // Установка текущего состояния сортировки
            CurrentState = sortOrder;
        }
    }
}

namespace MarriageAgency.ViewModels
{
    public enum SortState
    {
        No,                      // не сортировать
        CostAsc,                 // по стоимости по возрастанию
        CostDesc,                // по стоимости по убыванию
        ClientNameAsc,           // по имени клиента по возрастанию
        ClientNameDesc,          // по имени клиента по убыванию
        EmployeeNameAsc,         // по имени сотрудника по возрастанию
        EmployeeNameDesc,        // по имени сотрудника по убыванию
        AdditionalNameAsc,       // по имени дополнительной услуги по возрастанию
        AdditionalNameDesc       // по имени дополнительной услуги по убыванию
    }

    public class SortViewModel
    {
        public SortState CostSort { get; set; }              // Сортировка по стоимости
        public SortState ClientNameSort { get; set; }        // Сортировка по имени клиента
        public SortState EmployeeNameSort { get; set; }      // Сортировка по имени сотрудника
        public SortState AdditionalNameSort { get; set; }    // Сортировка по имени дополнительной услуги
        public SortState CurrentState { get; set; }          // Текущее состояние сортировки

        public SortViewModel(SortState sortOrder)
        {
            // Установка сортировки для стоимости
            CostSort = sortOrder == SortState.CostAsc ? SortState.CostDesc : SortState.CostAsc;

            // Установка сортировки для имени клиента
            ClientNameSort = sortOrder == SortState.ClientNameAsc ? SortState.ClientNameDesc : SortState.ClientNameAsc;

            // Установка сортировки для имени сотрудника
            EmployeeNameSort = sortOrder == SortState.EmployeeNameAsc ? SortState.EmployeeNameDesc : SortState.EmployeeNameAsc;

            // Установка сортировки для имени дополнительной услуги
            AdditionalNameSort = sortOrder == SortState.AdditionalNameAsc ? SortState.AdditionalNameDesc : SortState.AdditionalNameAsc;

            // Установка текущего состояния сортировки
            CurrentState = sortOrder;
        }
    }
}

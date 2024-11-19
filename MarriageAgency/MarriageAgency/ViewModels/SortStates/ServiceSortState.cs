namespace MarriageAgency.ViewModels.SortStates
{
    public enum ServiceSortState
    {
        No,                      // не сортировать
        ClientNameAsc,           // по имени клиента по возрастанию
        ClientNameDesc,          // по имени клиента по убыванию
        EmployeeNameAsc,         // по имени сотрудника по возрастанию
        EmployeeNameDesc,        // по имени сотрудника по убыванию
        AdditionalNameAsc,       // по имени дополнительной услуги по возрастанию
        AdditionalNameDesc,      // по имени дополнительной услуги по убыванию
        CostAsc,                 // по стоимости по возрастанию
        CostDesc,                // по стоимости по убыванию
    }
}

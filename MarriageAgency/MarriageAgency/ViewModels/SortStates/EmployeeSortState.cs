namespace MarriageAgency.ViewModels.SortStates
{
    public enum EmployeeSortState
    {
        No,
        EmployeeNameAsc,         // по имени сотрудника по возрастанию
        EmployeeNameDesc,        // по имени сотрудника по убыванию
        EmployeeLastNameAsc,     // по фамилии сотрудника по возрастанию
        EmployeeLastNameDesc,    // по фамилии сотрудника по убыванию
        EmployeeMiddleNameAsc,   // по отчеству сотрудника по возрастанию
        EmployeeMiddleNameDesc,  // по отчеству сотрудника по убыванию
        BirthDateAsc,            // по дате рождения по возрастанию
        BirthDateDesc,           // по дате рождения по убыванию
    }
}

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
        EmployeeLastNameAsc,     // по фамилии сотрудника по возрастанию
        EmployeeLastNameDesc,    // по фамилии сотрудника по убыванию
        EmployeeMiddleNameAsc,   // по отчеству сотрудника по возрастанию
        EmployeeMiddleNameDesc,  // по отчеству сотрудника по убыванию
        AdditionalNameAsc,       // по имени дополнительной услуги по возрастанию
        AdditionalNameDesc,      // по имени дополнительной услуги по убыванию
        BirthDateAsc,            // по дате рождения по возрастанию
        BirthDateDesc,           // по дате рождения по убыванию
        GenderAsc,               // по полу по возрастанию
        GenderDesc,              // по полу по убыванию
        NationalityAsc,          // по национальности по возрастанию
        NationalityDesc,         // по национальности по убыванию
        NationalityNameAsc,      // по имени национальности по возрастанию
        NationalityNameDesc,     // по имени национальности по убыванию
        ZodiacSignNameAsc,       // по знаку зодиака по возрастанию
        ZodiacSignNameDesc,      // по знаку зодиака по убыванию
        AgeAsc,                  // по возрасту по возрастанию
        AgeDesc,                 // по возрасту по убыванию
        HobbiesAsc,              // по хобби по возрастанию
        HobbiesDesc,              // по хобби по убыванию
        ContactAddressAsc,
        ContactAddressDesc,
    }

    public class SortViewModel
    {
        public SortState CostSort { get; set; }              // Сортировка по стоимости
        public SortState ClientNameSort { get; set; }        // Сортировка по имени клиента
        public SortState EmployeeNameSort { get; set; }      // Сортировка по имени сотрудника
        public SortState EmployeeLastNameSort { get; set; }  // Сортировка по фамилии сотрудника
        public SortState EmployeeMiddleNameSort { get; set; } // Сортировка по отчеству сотрудника
        public SortState AdditionalNameSort { get; set; }    // Сортировка по имени дополнительной услуги
        public SortState BirthDateSort { get; set; }         // Сортировка по дате рождения
        public SortState GenderSort { get; set; }            // Сортировка по полу
        public SortState NationalitySort { get; set; }       // Сортировка по национальности
        public SortState NationalityNameSort { get; set; }   // Сортировка по имени национальности
        public SortState ZodiacSignNameSort { get; set; }    // Сортировка по знаку зодиака
        public SortState AgeSort { get; set; }               // Сортировка по возрасту
        public SortState HobbiesSort { get; set; }           // Сортировка по хобби
        public SortState ContactAddressSort { get; set; }    // Сортировка по хобби
        public SortState CurrentState { get; set; }          // Текущее состояние сортировки

        public SortViewModel(SortState sortOrder)
        {
            // Установка сортировки для стоимости
            CostSort = sortOrder == SortState.CostAsc ? SortState.CostDesc : SortState.CostAsc;

            // Установка сортировки для имени клиента
            ClientNameSort = sortOrder == SortState.ClientNameAsc ? SortState.ClientNameDesc : SortState.ClientNameAsc;

            // Установка сортировки для имени сотрудника
            EmployeeNameSort = sortOrder == SortState.EmployeeNameAsc ? SortState.EmployeeNameDesc : SortState.EmployeeNameAsc;

            // Установка сортировки для фамилии сотрудника
            EmployeeLastNameSort = sortOrder == SortState.EmployeeLastNameAsc ? SortState.EmployeeLastNameDesc : SortState.EmployeeLastNameAsc;

            // Установка сортировки для отчества сотрудника
            EmployeeMiddleNameSort = sortOrder == SortState.EmployeeMiddleNameAsc ? SortState.EmployeeMiddleNameDesc : SortState.EmployeeMiddleNameAsc;

            // Установка сортировки для имени дополнительной услуги
            AdditionalNameSort = sortOrder == SortState.AdditionalNameAsc ? SortState.AdditionalNameDesc : SortState.AdditionalNameAsc;

            // Установка сортировки для даты рождения
            BirthDateSort = sortOrder == SortState.BirthDateAsc ? SortState.BirthDateDesc : SortState.BirthDateAsc;

            // Установка сортировки для пола
            GenderSort = sortOrder == SortState.GenderAsc ? SortState.GenderDesc : SortState.GenderAsc;

            // Установка сортировки для национальности
            NationalitySort = sortOrder == SortState.NationalityAsc ? SortState.NationalityDesc : SortState.NationalityAsc;

            // Установка сортировки для знака зодиака
            ZodiacSignNameSort = sortOrder == SortState.ZodiacSignNameAsc ? SortState.ZodiacSignNameDesc : SortState.ZodiacSignNameAsc;

            // Установка сортировки для возраста
            AgeSort = sortOrder == SortState.AgeAsc ? SortState.AgeDesc : SortState.AgeAsc;

            // Установка сортировки для хобби
            HobbiesSort = sortOrder == SortState.HobbiesAsc ? SortState.HobbiesDesc : SortState.HobbiesAsc;

            NationalityNameSort = sortOrder == SortState.NationalityNameAsc ? SortState.NationalityNameDesc : SortState.NationalityNameAsc;

            ContactAddressSort = sortOrder == SortState.ContactAddressAsc ? SortState.ContactAddressDesc : SortState.ContactAddressAsc;

            // Установка текущего состояния сортировки
            CurrentState = sortOrder;
        }
    }
}

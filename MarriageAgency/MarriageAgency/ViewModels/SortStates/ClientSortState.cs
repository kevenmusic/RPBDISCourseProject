namespace MarriageAgency.ViewModels.SortStates
{
    public enum ClientSortState
    {
        No,                      // не сортировать
        ClientNameAsc,           // по имени клиента по возрастанию
        ClientNameDesc,          // по имени клиента по убыванию
        GenderAsc,               // по полу по возрастанию
        GenderDesc,              // по полу по убыванию
        HobbiesAsc,              // по хобби по возрастанию
        HobbiesDesc,             // по хобби по убыванию
        NationalityNameAsc,      // по имени национальности по возрастанию
        NationalityNameDesc,     // по имени национальности по убыванию
        ZodiacSignNameAsc,       // по знаку зодиака по возрастанию
        ZodiacSignNameDesc,      // по знаку зодиака по убыванию
        AgeAsc,                  // по возрасту по возрастанию
        AgeDesc,                 // по возрасту по убыванию
    }
}

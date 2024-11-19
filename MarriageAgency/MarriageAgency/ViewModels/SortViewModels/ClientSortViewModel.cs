using MarriageAgency.ViewModels.SortStates;

namespace MarriageAgency.ViewModels.SortViewModels
{
    public class ClientSortViewModel
    {
        public ClientSortState CurrentState { get; set; }          // Текущее состояние сортировки
        public ClientSortState ClientNameSort { get; set; }        // Сортировка по имени клиента
        public ClientSortState GenderSort { get; set; }            // Сортировка по полу
        public ClientSortState HobbiesSort { get; set; }           // Сортировка по хобби
        public ClientSortState NationalityNameSort { get; set; }   // Сортировка по имени национальности
        public ClientSortState ZodiacSignNameSort { get; set; }    // Сортировка по знаку зодиака
        public ClientSortState AgeSort { get; set; }               // Сортировка по возрасту
        public ClientSortViewModel(ClientSortState sortOrder) 
        {

            // Установка сортировки для имени клиента
            ClientNameSort = sortOrder == ClientSortState.ClientNameAsc ? ClientSortState.ClientNameDesc : ClientSortState.ClientNameAsc;
            // Установка сортировки для пола
            GenderSort = sortOrder == ClientSortState.GenderAsc ? ClientSortState.GenderDesc : ClientSortState.GenderAsc;

            // Установка сортировки для знака зодиака
            ZodiacSignNameSort = sortOrder == ClientSortState.ZodiacSignNameAsc ? 
                ClientSortState.ZodiacSignNameDesc : ClientSortState.ZodiacSignNameAsc;

            // Установка сортировки для возраста
            AgeSort = sortOrder == ClientSortState.AgeAsc ? ClientSortState.AgeDesc : ClientSortState.AgeAsc;

            // Установка сортировки для хобби
            HobbiesSort = sortOrder == ClientSortState.HobbiesAsc ? ClientSortState.HobbiesDesc : ClientSortState.HobbiesAsc;

            // Установка сортировки для национальности
            NationalityNameSort = sortOrder == ClientSortState.NationalityNameAsc ? 
                ClientSortState.NationalityNameDesc : ClientSortState.NationalityNameAsc;

            // Установка текущего состояния сортировки
            CurrentState = sortOrder;
        }
    }
}

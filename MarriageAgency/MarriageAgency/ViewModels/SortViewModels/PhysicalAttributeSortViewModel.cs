using MarriageAgency.ViewModels.SortStates;
namespace MarriageAgency.ViewModels.SortViewModels
{
    public class PhysicalAttributeSortViewModel
    {
        public PhysicalAttributeSortState HobbiesSort { get; set; }           // Сортировка по хобби
        public PhysicalAttributeSortState CurrentState { get; set; }          // Текущее состояние сортировки
        public PhysicalAttributeSortViewModel(PhysicalAttributeSortState sortOrder)
        {
            // Установка сортировки для хобби
            HobbiesSort = sortOrder == PhysicalAttributeSortState.HobbiesAsc ? PhysicalAttributeSortState.HobbiesDesc : PhysicalAttributeSortState.HobbiesAsc;
            // Установка текущего состояния сортировки
            CurrentState = sortOrder;
        }
    }
}

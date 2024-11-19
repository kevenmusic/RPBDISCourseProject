using MarriageAgency.ViewModels.SortStates;
namespace MarriageAgency.ViewModels.SortViewModels
{
    public class NationalitySortViewModel
    {
        public NationalitySortState NationalityNameSort { get; set; }   // Сортировка по имени национальности
        public NationalitySortState CurrentState { get; set; }          // Текущее состояние сортировки

        public NationalitySortViewModel(NationalitySortState sortOrder)
        {
            // Установка сортировки для национальности
            NationalityNameSort = sortOrder == NationalitySortState.NationalityNameAsc ? NationalitySortState.NationalityNameDesc : NationalitySortState.NationalityNameAsc;
            // Установка текущего состояния сортировки
            CurrentState = sortOrder;
        }
    }
}

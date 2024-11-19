using MarriageAgency.ViewModels.SortStates;

namespace MarriageAgency.ViewModels.SortViewModels
{
    public class AdditionalServiceSortViewModel
    {
        public AdditionalServiceSortState CurrentState { get; set; }          // Текущее состояние сортировки
        public AdditionalServiceSortState AdditionalNameSort { get; set; }    // Сортировка по имени дополнительной услуги
        public AdditionalServiceSortViewModel(AdditionalServiceSortState sortOrder)
        {
            CurrentState = sortOrder;
            // Установка сортировки для имени дополнительной услуги
            AdditionalNameSort = sortOrder == AdditionalServiceSortState.AdditionalNameAsc ? 
                AdditionalServiceSortState.AdditionalNameDesc : AdditionalServiceSortState.AdditionalNameAsc;
        }
    }
}

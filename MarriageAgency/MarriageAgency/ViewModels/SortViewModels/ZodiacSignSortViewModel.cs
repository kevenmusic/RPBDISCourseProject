using MarriageAgency.ViewModels.SortStates;

namespace MarriageAgency.ViewModels.SortViewModels
{
    public class ZodiacSignSortViewModel
    {
        public ZodiacSignSortState ZodiacSignNameSort { get; set; }    // Сортировка по знаку зодиака
        public ZodiacSignSortState CurrentState { get; set; }          // Текущее состояние сортировки
        public ZodiacSignSortViewModel(ZodiacSignSortState sortOrder)
        {
            // Установка сортировки для знака зодиака
            ZodiacSignNameSort = sortOrder == ZodiacSignSortState.ZodiacSignNameAsc ?
                ZodiacSignSortState.ZodiacSignNameDesc : ZodiacSignSortState.ZodiacSignNameAsc;
            // Установка текущего состояния сортировки
            CurrentState = sortOrder;
        }
    }
}

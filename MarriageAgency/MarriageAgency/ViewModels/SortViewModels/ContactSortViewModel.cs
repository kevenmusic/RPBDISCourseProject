using MarriageAgency.ViewModels.SortStates;
namespace MarriageAgency.ViewModels.SortViewModels
{
    public class ContactSortViewModel
    {
        public ContactSortState ContactAddressSort { get; set; }    // Сортировка по хобби
        public ContactSortState CurrentState { get; set; }          // Текущее состояние сортировки

        public ContactSortViewModel(ContactSortState sortOrder)
        {
            ContactAddressSort = sortOrder == ContactSortState.ContactAddressAsc ? 
                ContactSortState.ContactAddressDesc : ContactSortState.ContactAddressAsc;
            // Установка текущего состояния сортировки
            CurrentState = sortOrder;
        }
    }
}

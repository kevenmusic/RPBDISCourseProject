using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels.ClientsViewModel;

namespace MarriageAgency.ViewModels.ContactsViewModel
{
    public class ContactsViewModel
    {
        public IEnumerable<Contact> Contacts { get; set; }

        public FilterContactsViewModel FilterContactsViewModel { get; set; }

        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}

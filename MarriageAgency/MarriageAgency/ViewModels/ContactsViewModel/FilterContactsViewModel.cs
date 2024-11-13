using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.ViewModels.ContactsViewModel
{
    public class FilterContactsViewModel
    {
        [Display(Name = "Адрес")]
        public string? ContactAddress { get; set; }

        [Display(Name = "Телефон")]
        public string? Phone { get; set; }

        [Display(Name = "Паспортные данные")]
        public string? PassportData { get; set; }
    }
}

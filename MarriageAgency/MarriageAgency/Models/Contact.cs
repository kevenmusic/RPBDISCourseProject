using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.Models
{
    public partial class Contact
    {
        [Display(Name = "Код клиента")]
        public int ClientId { get; set; }

        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [Display(Name = "Телефон")]
        public string? Phone { get; set; }

        [Display(Name = "Паспортные данные")]
        public string? PassportData { get; set; }

        [Display(Name = "Клиент")]
        public virtual Client Client { get; set; } = null!;
    }
}
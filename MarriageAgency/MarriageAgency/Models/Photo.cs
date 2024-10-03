using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.Models
{
    public partial class Photo
    {
        [Display(Name = "Код клиента")]
        public int ClientId { get; set; }

        [Display(Name = "Фото клиента")]
        public string ClientPhoto { get; set; } = null!;

        [Display(Name = "Клиент")]
        public virtual Client Client { get; set; } = null!;
    }
}
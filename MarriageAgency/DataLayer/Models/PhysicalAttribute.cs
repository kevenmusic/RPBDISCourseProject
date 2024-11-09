using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.DataLayer.Models
{
    public partial class PhysicalAttribute
    {
        [Key]
        [Display(Name = "Код клиента")]
        public int ClientId { get; set; }

        [Display(Name = "Возраст")]
        public int? Age { get; set; }

        [Display(Name = "Рост")]
        public decimal? Height { get; set; }

        [Display(Name = "Вес")]
        public decimal? Weight { get; set; }

        [Display(Name = "Количество детей")]
        public int? ChildrenCount { get; set; }

        [Display(Name = "Семейное положение")]
        public string? MaritalStatus { get; set; }

        [Display(Name = "Вредные привычки")]
        public string? BadHabits { get; set; }

        [Display(Name = "Хобби")]
        public string? Hobbies { get; set; }

        [Display(Name = "Клиент")]
        public virtual Client Client { get; set; } = null!;
    }
}
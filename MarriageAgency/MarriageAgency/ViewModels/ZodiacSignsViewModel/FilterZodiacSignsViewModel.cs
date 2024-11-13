using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.ViewModels.ZodiacSignsViewModel
{
    public class FilterZodiacSignsViewModel
    {
        [Display(Name = "Название знака")]
        public string ZodiacSignName { get; set; } = null!;

        [Display(Name = "Описание")]
        public string? Description { get; set; }
    }
}

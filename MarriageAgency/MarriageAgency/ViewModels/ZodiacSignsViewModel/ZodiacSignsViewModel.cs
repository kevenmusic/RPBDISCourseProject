using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels.SortViewModels;

namespace MarriageAgency.ViewModels.ZodiacSignsViewModel
{
    public class ZodiacSignsViewModel
    {
        public IEnumerable<ZodiacSign> ZodiacSigns { get; set; }

        //Свойство для фильтрации
        public FilterZodiacSignsViewModel FilterZodiacSignsViewModel { get; set; }
        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }

        public ZodiacSignSortViewModel SortViewModel { get; set; }
    }
}

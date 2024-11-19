using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels.SortViewModels;


namespace MarriageAgency.ViewModels.NationalitiesViewModel
{
    public class NationalitiesViewModel
    {
        public IEnumerable<Nationality> Nationalities { get; set; }

        public FilterNationalitiesViewModel FilterNationalitiesViewModel { get; set; }

        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        // Порядок сортировки
        public NationalitySortViewModel SortViewModel { get; set; }
    }
}

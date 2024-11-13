using MarriageAgency.DataLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.ViewModels.NationalitiesViewModel
{
    public class NationalitiesViewModel
    {
        public IEnumerable<Nationality> Nationalities { get; set; }

        public FilterNationalitiesViewModel FilterNationalitiesViewModel { get; set; }

        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}

using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels.SortViewModels;


namespace MarriageAgency.ViewModels.PhysicalAttributesViewModel
{
    public class PhysicalAttributesViewModel
    {
        public IEnumerable<PhysicalAttribute> PhysicalAttributes { get; set; }

        public FilterPhysicalAttributesViewModel FilterPhysicalAttributesViewModel { get; set; }

        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        // Порядок сортировки
        public PhysicalAttributeSortViewModel SortViewModel { get; set; }
    }
}

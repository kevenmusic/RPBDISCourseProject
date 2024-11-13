using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels.ClientsViewModel;

namespace MarriageAgency.ViewModels.PhysicalAttributesViewModel
{
    public class PhysicalAttributesViewModel
    {
        public IEnumerable<PhysicalAttribute> PhysicalAttributes { get; set; }

        public FilterPhysicalAttributesViewModel FilterPhysicalAttributesViewModel { get; set; }

        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}

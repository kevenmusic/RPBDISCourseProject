using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels.ClientsViewModel;

namespace MarriageAgency.ViewModels.AdditionalServicesViewModel
{
    public class AdditionalServicesViewModel
    {
        public IEnumerable<AdditionalService> AdditionalServices { get; set; }

        public FilterAdditionalServicesViewModel FilterAdditionalServicesViewModel { get; set; }

        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}

using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels.SortViewModels;

namespace MarriageAgency.ViewModels.ServicesViewModel
{
    public class ServicesViewModel
    {
        public IEnumerable<Service> Services { get; set; }

        //Свойство для фильтрации
        public FilterServicesViewModel FilterServicesViewModel { get; set; }
        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }

        public ServiceSortViewModel SortViewModel { get; set; }
    }
}
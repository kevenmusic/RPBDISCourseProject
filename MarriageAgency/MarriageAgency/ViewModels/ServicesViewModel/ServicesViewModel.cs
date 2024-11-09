using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using MarriageAgency.DataLayer.Models;

namespace MarriageAgency.ViewModels.ServicesViewModel
{
    public class ServicesViewModel
    {
        public IEnumerable<Service> Services { get; set; }

        //Свойство для фильтрации
        public FilterServicesViewModel FilterServicesViewModel { get; set; }
        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }

        public SortViewModel SortViewModel { get; set; }
    }
}
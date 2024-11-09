using MarriageAgency.DataLayer.Models;
using System.ComponentModel.DataAnnotations;

namespace MarriageAgency.ViewModels.ClientsViewModel
{
    public class ClientsViewModel
    {
        public IEnumerable<Client> Clients { get; set; }

        public FilterClientsViewModel FilterClientsViewModel { get; set; }

        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        // Порядок сортировки
        public SortViewModel SortViewModel { get; set; }
    }
}
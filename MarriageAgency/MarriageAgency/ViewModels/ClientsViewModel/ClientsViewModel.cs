using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels.SortViewModels;

namespace MarriageAgency.ViewModels.ClientsViewModel
{
    public class ClientsViewModel
    {
        public IEnumerable<Client> Clients { get; set; }

        public FilterClientsViewModel FilterClientsViewModel { get; set; }

        //Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
        // Порядок сортировки
        public ClientSortViewModel SortViewModel { get; set; }
    }
}
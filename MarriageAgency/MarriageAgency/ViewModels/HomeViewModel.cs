using System.Threading.Tasks;
using MarriageAgency.DataLayer.Models;
using Microsoft.Identity.Client.Extensions.Msal;
using MarriageAgency.ViewModels.ServicesViewModel;

namespace MarriageAgency.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Client> Clients { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<FilterServicesViewModel> Services { get; set; }
    }
}

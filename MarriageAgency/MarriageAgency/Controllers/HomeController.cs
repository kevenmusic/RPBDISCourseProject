using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels;
using MarriageAgency.ViewModels.ServicesViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MarriageAgency.Controllers
{
    public class HomeController(MarriageAgencyContext db) : Controller
    {
        private readonly MarriageAgencyContext _db = db;

        public IActionResult Index()
        {
            int numberRows = 10;
            List<Client> clients = [.. _db.Clients.Take(numberRows)];
            List<Employee> employees = [.. _db.Employees.Take(numberRows)];
            List<FilterServicesViewModel> services = [.. _db.Services
                .OrderByDescending(d => d.Date)
                .Select(s => new FilterServicesViewModel {
                    ServiceId = s.ServiceId,
                    EmployeeName = s.Employee.FirstName,
                    ClientName = s.Client.FirstName,
                    AdditionalServiceName = s.AdditionalService.Name,
                    Date = s.Date,
                    Cost = s.Cost,
                })
                .Take(numberRows)];

            HomeViewModel homeViewModel = new() { Clients = clients, Employees = employees, Services = services };
            return View(homeViewModel);
        }
    }
}
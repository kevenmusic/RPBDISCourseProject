﻿using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.Infrastructure;
using MarriageAgency.Infrastructure.Filters;
using MarriageAgency.ViewModels;
using MarriageAgency.ViewModels.ServicesViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarriageAgency.ViewModels.SortStates;
using MarriageAgency.ViewModels.SortViewModels;

namespace MarriageAgency.Controllers
{
    public class ServicesController : Controller
    {
        private readonly MarriageAgencyContext _context;
        private readonly int pageSize = 10;   // количество элементов на странице

        public ServicesController(MarriageAgencyContext context, IConfiguration appConfig = null)
        {
            _context = context;
            if (appConfig != null)
            {
                pageSize = int.Parse(appConfig["Parameters:PageSize"]);
            }
        }

        // GET: Services
        [SetToSession("Service")]
        [Authorize]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 264)]
        public async Task<IActionResult> Index(FilterServicesViewModel service, ServiceSortState sortOrder = ServiceSortState.No, int page = 1)
        {
            if (service.ClientName == null && service.EmployeeName == null 
                && service.AdditionalServiceName == null && service.MinCost == null  && service.MaxCost == null)
            {
                // Считывание данных из сессии
                if (HttpContext != null)
                {
                    // Считывание данных из сессии
                    var sessionService = Infrastructure.SessionExtensions.Get(HttpContext.Session, "Service");
                    if (sessionService != null)
                        service = Transformations.DictionaryToObject<FilterServicesViewModel>(sessionService);
                }
            }

            // Сортировка и фильтрация данных
            IQueryable<Service> marriageAgencyContext = _context.Services
                .Include(o => o.Employee)
                .Include(o => o.Client)
                .Include(o => o.AdditionalService);

            marriageAgencyContext = Sort_Search(marriageAgencyContext, sortOrder, service.ClientName ?? "",
                service.EmployeeName ?? "", service.AdditionalServiceName ?? "", service.MinCost, service.MaxCost);

            // Разбиение на страницы
            var count = await marriageAgencyContext.CountAsync();
            marriageAgencyContext = marriageAgencyContext.Skip((page - 1) * pageSize).Take(pageSize);

            // Получение данных асинхронно
            var servicesList = await marriageAgencyContext.ToListAsync();
            ViewData["Name"] = new SelectList(_context.AdditionalServices, "Name", "Name");
            // Формирование модели для передачи представлению
            ServicesViewModel services = new()
            {
                Services = servicesList,
                SortViewModel = new ServiceSortViewModel(sortOrder),
                FilterServicesViewModel = service,
                PageViewModel = new PageViewModel(count, page, pageSize)
            };

            return View(services);
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(o => o.Employee)
                .Include(o => o.Client)
                .Include(o => o.AdditionalService)
                .SingleOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Services/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "FirstName");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName");
            ViewData["AdditionalServiceId"] = new SelectList(_context.AdditionalServices, "AdditionalServiceId", "Name");

            return View();
        }

        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("ServiceId,AdditionalServiceId, EmployeeId, ClientId, Date, Cost")] Service service)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                _context.Add(service);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Services/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var service = await _context.Services.SingleOrDefaultAsync(m => m.ServiceId == id);

            if (service == null)
            {
                return NotFound();
            }

            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "FirstName", service.ClientId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", service.EmployeeId);
            ViewData["AdditionalServiceId"] = new SelectList(_context.AdditionalServices, "AdditionalServiceId", "Name", service.AdditionalServiceId);
            return View(service);
        }

        // POST: Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,AdditionalServiceId,ClientId,EmployeeId,Date,Cost")] Service service)
        {
            if (id != service.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ServiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "FirstName", service.ClientId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", service.EmployeeId);
            ViewData["AdditionalServiceId"] = new SelectList(_context.AdditionalServices, "AdditionalServiceId", "Name", service.AdditionalServiceId);
            return View(service);
        }

        // GET: Services/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(o => o.Employee)
                .Include(o => o.Client)
                .Include(o => o.AdditionalService)
                .SingleOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.SingleOrDefaultAsync(m => m.ServiceId == id);
            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceId == id);
        }

        private static IQueryable<Service> Sort_Search(
         IQueryable<Service> services,
         ServiceSortState sortOrder,
         string searchClientName,
         string searchEmployeeName,
         string additionalServiceName,
         decimal? minCost,
         decimal? maxCost)
        {
            switch (sortOrder)
            {
                case ServiceSortState.ClientNameAsc:
                    services = services.OrderBy(s => s.Client.FirstName);
                    break;
                case ServiceSortState.ClientNameDesc:
                    services = services.OrderByDescending(s => s.Client.FirstName);
                    break;
                case ServiceSortState.EmployeeNameAsc:
                    services = services.OrderBy(s => s.Employee.FirstName);
                    break;
                case ServiceSortState.EmployeeNameDesc:
                    services = services.OrderByDescending(s => s.Employee.FirstName);
                    break;
                case ServiceSortState.AdditionalNameAsc:
                    services = services.OrderBy(s => s.AdditionalService.Name);
                    break;
                case ServiceSortState.AdditionalNameDesc:
                    services = services.OrderByDescending(s => s.AdditionalService.Name);
                    break;
                case ServiceSortState.CostAsc:
                    services = services.OrderBy(s => s.Cost);
                    break;
                case ServiceSortState.CostDesc:
                    services = services.OrderByDescending(s => s.Cost);
                    break;

            }

            services = services.Include(o => o.Client)
                   .Include(o => o.Employee)
                   .Include(o => o.AdditionalService)
                   .Where(o => (string.IsNullOrEmpty(searchClientName) || o.Client.FirstName.Contains(searchClientName))
                           && (string.IsNullOrEmpty(searchEmployeeName) || o.Employee.FirstName.Contains(searchEmployeeName))
                           && (!minCost.HasValue || o.Cost >= minCost.Value)
                           && (!maxCost.HasValue || o.Cost <= maxCost.Value)
                           && (string.IsNullOrEmpty(additionalServiceName) || o.AdditionalService.Name.Contains(additionalServiceName)));

            return services;
        }

    }
}
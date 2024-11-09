using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.Infrastructure;
using MarriageAgency.Infrastructure.Filters;
using MarriageAgency.ViewModels;
using MarriageAgency.ViewModels.EmployeesViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarriageAgency.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MarriageAgencyContext _context;
        private readonly int pageSize = 10;   // количество элементов на странице 

        public EmployeesController(MarriageAgencyContext context, IConfiguration appConfig = null)
        {
            _context = context;
            if (appConfig != null)
            {
                pageSize = int.Parse(appConfig["Parameters:PageSize"]);
            }
        }

        // GET: Tanks
        [SetToSession("Employee")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 264)]
        public async Task<IActionResult> Index(FilterEmployeesViewModel employee, SortState sortOrder = SortState.No, int page = 1)
        {
            if (employee.EmployeeName == null)
            {
                // Считывание данных из сессии
                if (HttpContext != null)
                {
                    // Считывание данных из сессии
                    var sessionEmployee = Infrastructure.SessionExtensions.Get(HttpContext.Session, "Employee");

                    if (sessionEmployee != null)
                        employee = Transformations.DictionaryToObject<FilterEmployeesViewModel>(sessionEmployee);
                }
            }

            // Фильтрация данных
            IQueryable<Employee> marriageAgencyContext = _context.Employees;
            marriageAgencyContext = Sort_Search(marriageAgencyContext, sortOrder, employee.EmployeeName ?? "");

            // Разбиение на страницы
            var count = await marriageAgencyContext.CountAsync(); // Асинхронный подсчет
            marriageAgencyContext = marriageAgencyContext.Skip((page - 1) * pageSize).Take(pageSize);

            // Формирование модели для передачи представлению
            EmployeesViewModel employees = new()
            {
                Employees = await marriageAgencyContext.ToListAsync(), // Асинхронная загрузка данных
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterEmployeesViewModel = employee,
            };

            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tank = await _context.Employees
                .SingleOrDefaultAsync(m => m.EmployeeId == id);
            if (tank == null)
            {
                return NotFound();
            }

            return View(tank);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId, FirstName, MiddleName, LastName, Position, BirthDate")] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tank = await _context.Employees.SingleOrDefaultAsync(m => m.EmployeeId == id);
            if (tank == null)
            {
                return NotFound();
            }
            return View(tank);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId, FirstName, MiddleName, LastName, Position, BirthDate")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            else
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TankExists(employee.EmployeeId))
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
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tank = await _context.Employees
                .SingleOrDefaultAsync(m => m.EmployeeId == id);
            if (tank == null)
            {
                return NotFound();
            }

            return View(tank);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tank = await _context.Employees.SingleOrDefaultAsync(m => m.EmployeeId == id);
            _context.Employees.Remove(tank);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TankExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }

        private static IQueryable<Employee> Sort_Search(IQueryable<Employee> employees, SortState sortOrder, string EmployeeName)
        {
            switch (sortOrder)
            {
                case SortState.EmployeeNameAsc:
                    employees = employees.OrderBy(s => s.FirstName);
                    break;
                case SortState.EmployeeNameDesc:
                    employees = employees.OrderByDescending(s => s.FirstName);
                    break;
            }
            employees = employees.Where(o => o.FirstName.Contains(EmployeeName ?? ""));
            return employees;
        }
    }
}
using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.Infrastructure;
using MarriageAgency.Infrastructure.Filters;
using MarriageAgency.ViewModels;
using MarriageAgency.ViewModels.EmployeesViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarriageAgency.ViewModels.SortStates;
using MarriageAgency.ViewModels.SortViewModels;

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

        // GET: Employee
        [SetToSession("Employee")]
        [Authorize]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 264)]
        public async Task<IActionResult> Index(FilterEmployeesViewModel employee, EmployeeSortState sortOrder = EmployeeSortState.No, int page = 1, DateOnly? birthDate = null)
        {
            if (employee.EmployeeName == null && employee.EmployeeMiddleName == null 
                && employee.EmployeeLastName == null
                 && employee.BirthDate == null)
            {
                if (HttpContext != null)
                {
                    var sessionEmployee = Infrastructure.SessionExtensions.Get(HttpContext.Session, "Employee");
                    if (sessionEmployee != null)
                    {
                        employee = Transformations.DictionaryToObject<FilterEmployeesViewModel>(sessionEmployee);
                    }
                }
            }

            IQueryable<Employee> marriageAgencyContext = _context.Employees;

            marriageAgencyContext = Sort_Search(marriageAgencyContext, sortOrder, employee.EmployeeName ?? "", 
                employee.EmployeeLastName ?? "",
                employee.EmployeeMiddleName ?? "",
                employee.BirthDate);

            var count = await marriageAgencyContext.CountAsync();
            marriageAgencyContext = marriageAgencyContext.Skip((page - 1) * pageSize).Take(pageSize);

            EmployeesViewModel employees = new()
            {
                Employees = await marriageAgencyContext.ToListAsync(),
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new EmployeeSortViewModel(sortOrder),
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

            var employee = await _context.Employees
                .SingleOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.SingleOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
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
                    if (!EmployeeExists(employee.EmployeeId))
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .SingleOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.SingleOrDefaultAsync(m => m.EmployeeId == id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }

        private static IQueryable<Employee> Sort_Search(IQueryable<Employee> employees, EmployeeSortState sortOrder, string employeeName, string employeeLastName, string employeeMiddleName, DateOnly? birthDate)
        {
            if (!string.IsNullOrEmpty(employeeName))
            {
                employees = employees.Where(e => e.FirstName.Contains(employeeName));
            }

            if (!string.IsNullOrEmpty(employeeLastName))
            {
                employees = employees.Where(e => e.LastName.Contains(employeeLastName));
            }

            if (!string.IsNullOrEmpty(employeeMiddleName))
            {
                employees = employees.Where(e => e.MiddleName.Contains(employeeMiddleName));
            }

            if (birthDate.HasValue)
            {
                employees = employees.Where(e => e.BirthDate == birthDate.Value);
            }

            switch (sortOrder)
            {
                case EmployeeSortState.EmployeeNameAsc:
                    employees = employees.OrderBy(s => s.FirstName);
                    break;
                case EmployeeSortState.EmployeeNameDesc:
                    employees = employees.OrderByDescending(s => s.FirstName);
                    break;
                case EmployeeSortState.EmployeeLastNameAsc:
                    employees = employees.OrderBy(s => s.LastName);
                    break;
                case EmployeeSortState.EmployeeLastNameDesc:
                    employees = employees.OrderByDescending(s => s.LastName); 
                    break;
                case EmployeeSortState.EmployeeMiddleNameAsc:
                    employees = employees.OrderBy(s => s.MiddleName); 
                    break;
                case EmployeeSortState.EmployeeMiddleNameDesc:
                    employees = employees.OrderByDescending(s => s.MiddleName);
                    break;
                case EmployeeSortState.BirthDateAsc:
                    employees = employees.OrderBy(s => s.BirthDate);
                    break;
                case EmployeeSortState.BirthDateDesc:
                    employees = employees.OrderByDescending(s => s.BirthDate);
                    break;
                default:
                    break;
            }

            return employees;
        }
    }
}
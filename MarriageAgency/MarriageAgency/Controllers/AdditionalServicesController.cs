using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.Infrastructure.Filters;
using MarriageAgency.Infrastructure;
using MarriageAgency.ViewModels.EmployeesViewModel;
using MarriageAgency.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MarriageAgency.ViewModels.AdditionalServicesViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using MarriageAgency.ViewModels.SortStates;
using MarriageAgency.ViewModels.SortViewModels;

namespace MarriageAgency.Controllers
{
    public class AdditionalServicesController : Controller
    {
        private readonly MarriageAgencyContext _context;
        private readonly int pageSize = 10;   // количество элементов на странице 

        public AdditionalServicesController(MarriageAgencyContext context, IConfiguration appConfig = null)
        {
            _context = context;
            if (appConfig != null)
            {
                pageSize = int.Parse(appConfig["Parameters:PageSize"]);
            }
        }

        // GET: AdditionalServices
        [SetToSession("AdditionalService")]
        [Authorize]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 264)]
        public async Task<IActionResult> Index(FilterAdditionalServicesViewModel additionalService, AdditionalServiceSortState sortOrder = AdditionalServiceSortState.No, int page = 1)
        {
            if (additionalService.AdditionalServiceName == null)
            {
                if (HttpContext != null)
                {
                    var sessionAdditionalService = Infrastructure.SessionExtensions.Get(HttpContext.Session, "AdditionalService");
                    if (sessionAdditionalService != null)
                    {
                        additionalService = Transformations.DictionaryToObject<FilterAdditionalServicesViewModel>(sessionAdditionalService);
                    }
                }
            }

            IQueryable<AdditionalService> marriageAgencyContext = _context.AdditionalServices;

            marriageAgencyContext = Sort_Search(marriageAgencyContext, sortOrder, additionalService.AdditionalServiceName ?? "");

            var count = await marriageAgencyContext.CountAsync();
            marriageAgencyContext = marriageAgencyContext.Skip((page - 1) * pageSize).Take(pageSize);

            var additionalServicesList = await marriageAgencyContext.ToListAsync();

            ViewData["Name"] = new SelectList(_context.AdditionalServices, "Name", "Name");

            AdditionalServicesViewModel additionalServices = new()
            {
                AdditionalServices = additionalServicesList,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new AdditionalServiceSortViewModel(sortOrder),
                FilterAdditionalServicesViewModel = additionalService,
            };

            return View(additionalServices);
        }

        // GET: AdditionalServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var additionalService = await _context.AdditionalServices
                .SingleOrDefaultAsync(m => m.AdditionalServiceId == id);
            if (additionalService == null)
            {
                return NotFound();
            }

            return View(additionalService);
        }

        // GET: AdditionalServices/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdditionalServices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("AdditionalServiceId, Name, Description, Price")] AdditionalService additionalService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                _context.Add(additionalService);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }

        // GET: AdditionalServices/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var additionalService = await _context.AdditionalServices.SingleOrDefaultAsync(m => m.AdditionalServiceId == id);
            if (additionalService == null)
            {
                return NotFound();
            }
            return View(additionalService);
        }

        // POST: AdditionalServices/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("AdditionalServiceId, Name, Description, Price")] AdditionalService additionalService)
        {
            if (id != additionalService.AdditionalServiceId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(additionalService);
            }
            else
            {
                try
                {
                    _context.Update(additionalService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdditionalServiceExists(additionalService.AdditionalServiceId))
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

        // GET: AdditionalServices/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var additionalService = await _context.AdditionalServices
                .SingleOrDefaultAsync(m => m.AdditionalServiceId == id);
            if (additionalService == null)
            {
                return NotFound();
            }

            return View(additionalService);
        }

        // POST: AdditionalServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var additionalService = await _context.AdditionalServices.SingleOrDefaultAsync(m => m.AdditionalServiceId == id);
            _context.AdditionalServices.Remove(additionalService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdditionalServiceExists(int id)
        {
            return _context.AdditionalServices.Any(e => e.AdditionalServiceId == id);
        }

        private static IQueryable<AdditionalService> Sort_Search(IQueryable<AdditionalService> additionalServices, AdditionalServiceSortState sortOrder, string additionalServiceName)
        {
            if (!string.IsNullOrEmpty(additionalServiceName))
            {
                additionalServices = additionalServices.Where(e => e.Name.Contains(additionalServiceName));
            }

            switch (sortOrder)
            {
                case AdditionalServiceSortState.AdditionalNameAsc:
                    additionalServices = additionalServices.OrderBy(s => s.Name);
                    break;
                case AdditionalServiceSortState.AdditionalNameDesc:
                    additionalServices = additionalServices.OrderByDescending(s => s.Name);
                    break;
                default:
                    break;
            }

            return additionalServices;
        }
    }
}

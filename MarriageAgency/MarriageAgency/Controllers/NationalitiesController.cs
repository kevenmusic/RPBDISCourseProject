using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.Infrastructure.Filters;
using MarriageAgency.Infrastructure;
using MarriageAgency.ViewModels.ClientsViewModel;
using MarriageAgency.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarriageAgency.ViewModels.NationalitiesViewModel;

namespace MarriageAgency.Controllers
{
    public class NationalitiesController : Controller
    {
        private readonly MarriageAgencyContext _context;
        private readonly int pageSize = 10;   // количество элементов на странице 

        public NationalitiesController(MarriageAgencyContext context, IConfiguration appConfig = null)
        {
            _context = context;
            if (appConfig != null)
            {
                pageSize = int.Parse(appConfig["Parameters:PageSize"]);
            }
        }

        // GET: Nationalities
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 264)]
        [SetToSession("Nationality")]
        [Authorize]
        public async Task<IActionResult> Index(FilterNationalitiesViewModel nationality, SortState sortOrder = SortState.No, int page = 1)
        {
            if (nationality.NationalityName == null)
            {
                if (HttpContext != null)
                {
                    var sessionNationality = Infrastructure.SessionExtensions.Get(HttpContext.Session, "Nationality");

                    if (sessionNationality != null)
                        nationality = Transformations.DictionaryToObject<FilterNationalitiesViewModel>(sessionNationality);
                }
            }

            // Сортировка и фильтрация данных
            IQueryable<Nationality> marriageAgencyContext = _context.Nationalities;

            marriageAgencyContext = Sort_Search(marriageAgencyContext, sortOrder, nationality.NationalityName ?? "");

            // Разбиение на страницы
            var count = await marriageAgencyContext.CountAsync();
            marriageAgencyContext = marriageAgencyContext.Skip((page - 1) * pageSize).Take(pageSize);

            var nationalityList = await marriageAgencyContext.ToListAsync();

            ViewData["Name"] = new SelectList(_context.Nationalities, "Name", "Name");
            // Формирование модели для передачи представлению
            NationalitiesViewModel nationalities = new()
            {
                Nationalities = nationalityList,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterNationalitiesViewModel = nationality,
            };

            return View(nationalities);
        }

        // GET: Nationalities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nationality = await _context.Nationalities.SingleOrDefaultAsync(m => m.NationalityId == id);

            if (nationality == null)
            {
                return NotFound();
            }

            return View(nationality);
        }

        // GET: Nationalities/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Nationalities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("NationalityId, Name, Notes")] Nationality nationality)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                _context.Add(nationality);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Nationalities/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nationality = await _context.Nationalities.SingleOrDefaultAsync(m => m.NationalityId == id);
            if (nationality == null)
            {
                return NotFound();
            }
            return View(nationality);
        }

        // POST: Nationalities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("NationalityId, Name, Notes")] Nationality nationality)
        {
            if (id != nationality.NationalityId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(nationality);
            }
            else
            {
                try
                {
                    _context.Update(nationality);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NationalityExists(nationality.NationalityId))
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

        // GET: Nationalities/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nationality = await _context.Nationalities
                .SingleOrDefaultAsync(m => m.NationalityId == id);
            if (nationality == null)
            {
                return NotFound();
            }

            return View(nationality);
        }

        // POST: Nationalities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nationality = await _context.Nationalities.SingleOrDefaultAsync(m => m.NationalityId == id);
            _context.Nationalities.Remove(nationality);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NationalityExists(int id)
        {
            return _context.Nationalities.Any(e => e.NationalityId == id);
        }

        private static IQueryable<Nationality> Sort_Search(IQueryable<Nationality> nationalities, SortState sortOrder, string nationalityName)
        {
            if (!string.IsNullOrEmpty(nationalityName))
            {
                nationalities = nationalities.Where(e => e.Name.Contains(nationalityName));
            }

            switch (sortOrder)
            {
                case SortState.NationalityNameAsc:
                    nationalities = nationalities.OrderBy(s => s.Name);
                    break;
                case SortState.NationalityNameDesc:
                    nationalities = nationalities.OrderByDescending(s => s.Name);
                    break;
                default:
                    break;
            }

            return nationalities;
        }
    }
}

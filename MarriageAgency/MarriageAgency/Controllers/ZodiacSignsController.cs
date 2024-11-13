using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.Infrastructure;
using MarriageAgency.Infrastructure.Filters;
using MarriageAgency.ViewModels;
using MarriageAgency.ViewModels.NationalitiesViewModel;
using MarriageAgency.ViewModels.ZodiacSignsViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace MarriageAgency.Controllers
{
    public class ZodiacSignsController : Controller
    {
        private readonly MarriageAgencyContext _context;
        private readonly int pageSize = 10;   // количество элементов на странице

        public ZodiacSignsController(MarriageAgencyContext context, IConfiguration appConfig = null)
        {
            _context = context;
            if (appConfig != null)
            {
                pageSize = int.Parse(appConfig["Parameters:PageSize"]);
            }
        }

        // GET: ZodiacSigns
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 264)]
        [SetToSession("ZodiacSign")]
        [Authorize]
        public async Task<IActionResult> Index(FilterZodiacSignsViewModel zodiacSign, SortState sortOrder = SortState.No, int page = 1)
        {
            if (zodiacSign.ZodiacSignName == null)
            {
                if (HttpContext != null)
                {
                    var sessionZodiacSign = Infrastructure.SessionExtensions.Get(HttpContext.Session, "ZodiacSign");

                    if (sessionZodiacSign != null)
                        zodiacSign = Transformations.DictionaryToObject<FilterZodiacSignsViewModel>(sessionZodiacSign);
                }
            }

            // Сортировка и фильтрация данных
            IQueryable<ZodiacSign> marriageAgencyContext = _context.ZodiacSigns;

            marriageAgencyContext = Sort_Search(marriageAgencyContext, sortOrder, zodiacSign.ZodiacSignName ?? "");

            // Разбиение на страницы
            var count = await marriageAgencyContext.CountAsync();
            marriageAgencyContext = marriageAgencyContext.Skip((page - 1) * pageSize).Take(pageSize);

            var zodiacSignList = await marriageAgencyContext.ToListAsync();

            ViewData["Name"] = new SelectList(_context.ZodiacSigns, "Name", "Name");
            // Формирование модели для передачи представлению
            ZodiacSignsViewModel zodiacSigns = new()
            {
                ZodiacSigns = zodiacSignList,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterZodiacSignsViewModel = zodiacSign,
            };

            return View(zodiacSigns);
        }

        // GET: ZodiacSigns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zogiacSign = await _context.ZodiacSigns.SingleOrDefaultAsync(m => m.ZodiacSignId == id);

            if (zogiacSign == null)
            {
                return NotFound();
            }

            return View(zogiacSign);
        }

        // GET: ZodiacSigns/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ZodiacSigns/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("ZogiacSignId, Name, Description")] ZodiacSign zogiacSign)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                _context.Add(zogiacSign);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ZodiacSigns/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zogiacSign = await _context.ZodiacSigns.SingleOrDefaultAsync(m => m.ZodiacSignId == id);
            if (zogiacSign == null)
            {
                return NotFound();
            }
            return View(zogiacSign);
        }

        // POST: ZodiacSigns/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ZodiacSignId, Name, Description")] ZodiacSign zogiacSign)
        {
            if (id != zogiacSign.ZodiacSignId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(zogiacSign);
            }
            else
            {
                try
                {
                    _context.Update(zogiacSign);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZodiacSignsExists(zogiacSign.ZodiacSignId))
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

        // GET: ZodiacSigns/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zodiacSigns = await _context.ZodiacSigns
                .SingleOrDefaultAsync(m => m.ZodiacSignId == id);
            if (zodiacSigns == null)
            {
                return NotFound();
            }

            return View(zodiacSigns);
        }

        // POST: ZodiacSigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zodiacSigns = await _context.ZodiacSigns.SingleOrDefaultAsync(m => m.ZodiacSignId == id);
            _context.ZodiacSigns.Remove(zodiacSigns);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZodiacSignsExists(int id)
        {
            return _context.ZodiacSigns.Any(e => e.ZodiacSignId == id);
        }

        private static IQueryable<ZodiacSign> Sort_Search(IQueryable<ZodiacSign> zodiacSigns, SortState sortOrder, string zodiacSignName)
        {
            if (!string.IsNullOrEmpty(zodiacSignName))
            {
                zodiacSigns = zodiacSigns.Where(e => e.Name.Contains(zodiacSignName));
            }

            switch (sortOrder)
            {
                case SortState.ZodiacSignNameAsc:
                    zodiacSigns = zodiacSigns.OrderBy(s => s.Name);
                    break;
                case SortState.ZodiacSignNameDesc:
                    zodiacSigns = zodiacSigns.OrderByDescending(s => s.Name);
                    break;
                default:
                    break;
            }

            return zodiacSigns;
        }
    }
}

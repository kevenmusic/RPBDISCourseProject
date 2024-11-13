using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.Infrastructure.Filters;
using MarriageAgency.Infrastructure;
using MarriageAgency.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarriageAgency.ViewModels.PhysicalAttributesViewModel;

namespace MarriageAgency.Controllers
{
    public class PhysicalAttributesController : Controller
    {
        private readonly int pageSize = 10;   // количество элементов на странице
        private readonly MarriageAgencyContext _context;

        public PhysicalAttributesController(MarriageAgencyContext context, IConfiguration appConfig = null)
        {
            _context = context;
            if (appConfig != null)
            {
                pageSize = int.Parse(appConfig["Parameters:PageSize"]);
            }
        }

        // GET: PhysicalAttributes
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 264)]
        [SetToSession("PhysicalAttribute")]
        [Authorize]
        public async Task<IActionResult> Index(FilterPhysicalAttributesViewModel physicalAttribute, SortState sortOrder = SortState.No, int page = 1)
        {
            if (physicalAttribute.Hobbies == null)
            {
                if (HttpContext != null)
                {
                    var sessionPhysicalAttribute = Infrastructure.SessionExtensions.Get(HttpContext.Session, "PhysicalAttribute");

                    if (sessionPhysicalAttribute != null)
                        physicalAttribute = Transformations.DictionaryToObject<FilterPhysicalAttributesViewModel>(sessionPhysicalAttribute);
                }
            }

            // Сортировка и фильтрация данных
            IQueryable<PhysicalAttribute> marriageAgencyContext = _context.PhysicalAttributes;

            marriageAgencyContext = Sort_Search(marriageAgencyContext, sortOrder, physicalAttribute.Hobbies ?? "");

            // Разбиение на страницы
            var count = await marriageAgencyContext.CountAsync();
            marriageAgencyContext = marriageAgencyContext.Skip((page - 1) * pageSize).Take(pageSize);

            var physicalAttributeList = await marriageAgencyContext.ToListAsync();

            // Формирование модели для передачи представлению
            PhysicalAttributesViewModel physicalAttributes = new()
            {
                PhysicalAttributes = physicalAttributeList,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterPhysicalAttributesViewModel = physicalAttribute,
            };

            return View(physicalAttributes);
        }

        // GET: PhysicalAttributes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physicalAttribute = await _context.PhysicalAttributes
                .SingleOrDefaultAsync(m => m.ClientId == id);
            if (physicalAttribute == null)
            {
                return NotFound();
            }

            return View(physicalAttribute);
        }

        // GET: PhysicalAttributes/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PhysicalAttributes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("ClientId, Age, Height, Weight, ChildrenCount, MaritalStatus, BadHabits, Hobbies")] PhysicalAttribute physicalAttribute)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                _context.Add(physicalAttribute);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }

        // GET: PhysicalAttributes/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physicalAttribute = await _context.PhysicalAttributes.SingleOrDefaultAsync(m => m.ClientId == id);
            if (physicalAttribute == null)
            {
                return NotFound();
            }
            return View(physicalAttribute);
        }

        // POST: PhysicalAttributes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId, Age, Height, Weight, ChildrenCount, MaritalStatus, BadHabits, Hobbies")] PhysicalAttribute physicalAttribute)
        {
            if (id != physicalAttribute.ClientId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(physicalAttribute);
            }
            else
            {
                try
                {
                    _context.Update(physicalAttribute);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhysicalAttributeExists(physicalAttribute.ClientId))
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

        // GET: PhysicalAttributes/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var physicalAttribute = await _context.PhysicalAttributes
                .SingleOrDefaultAsync(m => m.ClientId == id);
            if (physicalAttribute == null)
            {
                return NotFound();
            }

            return View(physicalAttribute);
        }

        // POST: PhysicalAttributes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var physicalAttribute = await _context.PhysicalAttributes.SingleOrDefaultAsync(m => m.ClientId == id);
            _context.PhysicalAttributes.Remove(physicalAttribute);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhysicalAttributeExists(int id)
        {
            return _context.PhysicalAttributes.Any(e => e.ClientId == id);
        }

        private static IQueryable<PhysicalAttribute> Sort_Search(IQueryable<PhysicalAttribute> physicalAttributes, SortState sortOrder, string Hobbies)
        {
            if (!string.IsNullOrEmpty(Hobbies))
            {
                physicalAttributes = physicalAttributes.Where(e => e.Hobbies.Contains(Hobbies));
            }

            switch (sortOrder)
            {
                case SortState.HobbiesAsc:
                    physicalAttributes = physicalAttributes.OrderBy(s => s.Hobbies);
                    break;
                case SortState.HobbiesDesc:
                    physicalAttributes = physicalAttributes.OrderByDescending(s => s.Hobbies);
                    break;
                default:
                    break;
            }

            return physicalAttributes;
        }
    }
}

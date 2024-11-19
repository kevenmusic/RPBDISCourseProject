using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.Infrastructure.Filters;
using MarriageAgency.Infrastructure;
using MarriageAgency.ViewModels.NationalitiesViewModel;
using MarriageAgency.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarriageAgency.ViewModels.ContactsViewModel;
using MarriageAgency.ViewModels.SortStates;
using MarriageAgency.ViewModels.SortViewModels;

namespace MarriageAgency.Controllers
{
    public class ContactsController : Controller
    {
        private readonly int pageSize = 10;   // количество элементов на странице
        private readonly MarriageAgencyContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ContactsController(MarriageAgencyContext context, IConfiguration appConfig = null)
        {
            _context = context;
            if (appConfig != null)
            {
                pageSize = int.Parse(appConfig["Parameters:PageSize"]);
            }
        }

        // GET: Contacts
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 264)]
        [SetToSession("Contact")]
        [Authorize]
        public async Task<IActionResult> Index(FilterContactsViewModel contact, ContactSortState sortOrder = ContactSortState.No, int page = 1)
        {
            if (contact.ContactAddress == null)
            {
                if (HttpContext != null)
                {
                    var sessionContact = Infrastructure.SessionExtensions.Get(HttpContext.Session, "Contact");

                    if (sessionContact != null)
                        contact = Transformations.DictionaryToObject<FilterContactsViewModel>(sessionContact);
                }
            }

            // Сортировка и фильтрация данных
            IQueryable<Contact> marriageAgencyContext = _context.Contacts;

            marriageAgencyContext = Sort_Search(marriageAgencyContext, sortOrder, contact.ContactAddress ?? "");

            // Разбиение на страницы
            var count = await marriageAgencyContext.CountAsync();
            marriageAgencyContext = marriageAgencyContext.Skip((page - 1) * pageSize).Take(pageSize);

            var contactList = await marriageAgencyContext.ToListAsync();

            // Формирование модели для передачи представлению
            ContactsViewModel contacts = new()
            {
                Contacts = contactList,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new ContactSortViewModel(sortOrder),
                FilterContactsViewModel = contact,
            };

            return View(contacts);
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.SingleOrDefaultAsync(m => m.ClientId == id);

            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("ClientId, Address, Phone, PassportData")] Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Contacts/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.SingleOrDefaultAsync(m => m.ClientId == id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId, Address, Phone, PassportData")] Contact contact)
        {
            if (id != contact.ClientId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(contact);
            }
            else
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactsExists(contact.ClientId))
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

        // GET: Contacts/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .SingleOrDefaultAsync(m => m.ClientId == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.SingleOrDefaultAsync(m => m.ClientId == id);

            if (contact != null)
            {
                _context.Contacts.Remove(contact); // Удаляем запись из таблицы Contact
                await _context.SaveChangesAsync(); // Сохраняем изменения в базе данных
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ContactsExists(int id)
        {
            return _context.Contacts.Any(e => e.ClientId == id);
        }

        private static IQueryable<Contact> Sort_Search(IQueryable<Contact> contacts, ContactSortState sortOrder, string contactAddress)
        {
            if (!string.IsNullOrEmpty(contactAddress))
            {
                contacts = contacts.Where(e => e.Address.Contains(contactAddress));
            }

            switch (sortOrder)
            {
                case ContactSortState.ContactAddressAsc:
                    contacts = contacts.OrderBy(s => s.Address);
                    break;
                case ContactSortState.ContactAddressDesc:
                    contacts = contacts.OrderByDescending(s => s.Address);
                    break;
                default:
                    break;
            }

            return contacts;
        }
    }
}

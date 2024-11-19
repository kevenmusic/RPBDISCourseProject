using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.Infrastructure;
using MarriageAgency.Infrastructure.Filters;
using MarriageAgency.ViewModels;
using MarriageAgency.ViewModels.ClientsViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MarriageAgency.ViewModels.SortStates;
using MarriageAgency.ViewModels.SortViewModels;
public class ClientsController : Controller
{
    private readonly int pageSize = 10;   // количество элементов на странице
    private readonly MarriageAgencyContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ClientsController(MarriageAgencyContext context, IWebHostEnvironment webHost, IConfiguration appConfig = null)
    {
        _webHostEnvironment = webHost;
        _context = context;
        if (appConfig != null)
        {
            pageSize = int.Parse(appConfig["Parameters:PageSize"]);
        }
    }

    // GET: Clients
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 264)]
    [SetToSession("Client")]
    [Authorize]
    public async Task<IActionResult> Index(FilterClientsViewModel client, ClientSortState sortOrder = ClientSortState.No, int page = 1)
    {
        if (client.ClientName == null && client.Gender == null && client.NationalityName == null
            && client.ZodiacSignName == null && client.Age == null && client.Hobbies == null)
        {
            if (HttpContext != null)
            {
                var sessionClient = MarriageAgency.Infrastructure.SessionExtensions.Get(HttpContext.Session, "Client");

                if (sessionClient != null)
                    client = Transformations.DictionaryToObject<FilterClientsViewModel>(sessionClient);
            }
        }

        // Сортировка и фильтрация данных
        IQueryable<Client> marriageAgencyContext = _context.Clients
            .Include(c => c.Contact) // Загружаем связанные записи Contact
            .Include(c => c.Nationality) // Загружаем связанные записи Nationality
            .Include(c => c.PhysicalAttribute) // Загружаем связанные записи PhysicalAttribute
            .Include(c => c.ZodiacSign); // Загружаем связанные записи ZodiacSign

        marriageAgencyContext = Sort_Search(marriageAgencyContext, sortOrder, client.ClientName ?? "", 
            client.Gender ?? "", client.NationalityName ?? "", client.ZodiacSignName ?? "", 
            client.Age, client.Hobbies ?? "");

        // Разбиение на страницы
        var count = await marriageAgencyContext.CountAsync();
        marriageAgencyContext = marriageAgencyContext.Skip((page - 1) * pageSize).Take(pageSize);

        var clientsList = await marriageAgencyContext.ToListAsync();

        ViewData["Name"] = new SelectList(_context.Nationalities, "Name", "Name");
        ViewData["ZodiacSignName"] = new SelectList(_context.ZodiacSigns, "Name", "Name");

        // Формирование модели для передачи представлению
        ClientsViewModel clients = new()
        {
            Clients = clientsList,
            PageViewModel = new PageViewModel(count, page, pageSize),
            SortViewModel = new ClientSortViewModel(sortOrder),
            FilterClientsViewModel = client,
        };

        return View(clients);
    }

    // GET: Clients/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = await _context.Clients
            .Include(c => c.Contact) // Загружаем связанные записи Contact
            .Include(c => c.Nationality) // Загружаем связанные записи Nationality
            .Include(c => c.PhysicalAttribute) // Загружаем связанные записи PhysicalAttribute
            .Include(c => c.ZodiacSign) // Загружаем связанные записи ZodiacSign
            .SingleOrDefaultAsync(m => m.ClientId == id);

        if (client == null)
        {
            return NotFound();
        }


        return View(client);
    }

    // GET: Clients/Create
    [Authorize(Roles = "admin")]
    public IActionResult Create()
    {
        var nationality = _context.Nationalities;
        if (nationality != null) ViewData["NationalityId"] = new SelectList(nationality, "NationalityId", "Name");
        var zodiacSign = _context.ZodiacSigns;
        if (zodiacSign != null) ViewData["ZodiacSignId"] = new SelectList(zodiacSign, "ZodiacSignId", "Name");
        return View();
    }

    // POST: Clients/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create(Client client)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        else
        {
            string uniqueFileName = UploadedFile(client);
            client.ClientPhoto = uniqueFileName;
            _context.Add(client);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Clients/Edit/5
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = await _context.Clients.SingleOrDefaultAsync(m => m.ClientId == id);
        if (client == null)
        {
            return NotFound();
        }

        var zodiacSign = _context.ZodiacSigns;
        if (zodiacSign != null) ViewData["ZodiacSignId"] = new SelectList(zodiacSign, "ZodiacSignId", "Name", client.ZodiacSignId);
        var nationality = _context.Nationalities;
        if (nationality != null) ViewData["NationalityId"] = new SelectList(nationality, "NationalityId", "Name", client.NationalityId);

        return View(client);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Edit(int id, [Bind("ClientId,FirstName,LastName,MiddleName,Gender,BirthDate,ZodiacSignId,NationalityId,Profession,FrontImage,ClientPhoto")] Client client)
    {
        if (id != client.ClientId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Получаем существующего клиента из базы данных
                var existingClient = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.ClientId == client.ClientId);

                if (existingClient == null)
                {
                    return NotFound();
                }

                // Если файл был загружен, обновляем фото
                if (client.FrontImage != null)
                {
                    client.ClientPhoto = UploadedFile(client); // Загружаем файл
                }
                else
                {
                    // Если файл не был загружен, сохраняем старое фото
                    client.ClientPhoto = existingClient.ClientPhoto;
                }

                // Обновляем информацию о клиенте
                _context.Update(client);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(client.ClientId))
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

        ViewData["ZodiacSignId"] = new SelectList(_context.ZodiacSigns, "ZodiacSignId", "Name", client.ZodiacSignId);
        ViewData["NationalityId"] = new SelectList(_context.Nationalities, "NationalityId", "Name", client.NationalityId);
        return View(client);
    }

    // GET: Clients/Delete/5
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = await _context.Clients
            .Include(c => c.Contact) // Загружаем связанные записи Contact
            .Include(c => c.Nationality) // Загружаем связанные записи Nationality
            .Include(c => c.PhysicalAttribute) // Загружаем связанные записи PhysicalAttribute
            .Include(c => c.ZodiacSign) // Загружаем связанные записи ZodiacSign
            .SingleOrDefaultAsync(m => m.ClientId == id);
        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }

    // POST: Clients/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        // Находим клиента вместе с связанными сущностями
        var client = await _context.Clients
            .Include(c => c.Contact) // Загружаем связанные записи Contact
            .Include(c => c.Nationality) // Загружаем связанные записи Nationality
            .Include(c => c.PhysicalAttribute) // Загружаем связанные записи PhysicalAttribute
            .Include(c => c.ZodiacSign) // Загружаем связанные записи ZodiacSign
            .SingleOrDefaultAsync(m => m.ClientId == id);

        if (client != null)
        {
            // Удаляем связанные сущности, если они существуют
            if (client.Contact != null)
            {
                _context.Contacts.Remove(client.Contact); // Удаляем запись Contact
            }
            if (client.PhysicalAttribute != null)
            {
                _context.PhysicalAttributes.Remove(client.PhysicalAttribute); // Удаляем запись PhysicalAttribute
            }
            if (client.Nationality != null)
            {
                _context.Nationalities.Remove(client.Nationality); // Удаляем запись Nationality
            }
            if (client.ZodiacSign != null)
            {
                _context.ZodiacSigns.Remove(client.ZodiacSign); // Удаляем запись ZodiacSign
            }

            // Удаляем запись клиента
            _context.Clients.Remove(client);

            // Сохраняем изменения в базе данных
            await _context.SaveChangesAsync();
        }

        // Перенаправляем на страницу со списком клиентов
        return RedirectToAction(nameof(Index));
    }


    private bool ClientExists(int id)
    {
        return _context.Clients.Any(e => e.ClientId == id);
    }

    private string UploadedFile(Client client)
    {
        string uniqueFileName = null;
        if (client.FrontImage != null)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + client.FrontImage.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                client.FrontImage.CopyTo(fileStream);
            }
        }
        return uniqueFileName;
    }

    private static IQueryable<Client> Sort_Search(IQueryable<Client> clients, ClientSortState sortOrder, string ClientName, string Gender, string NationalityName, string ZodiacSignName, int? Age, string Hobbies)
    {
        clients = clients.Where(o => o.FirstName.Contains(ClientName ?? ""));

        if (!string.IsNullOrEmpty(Gender))
        {
            clients = clients.Where(o => o.Gender.Contains(Gender ?? ""));
        }

        if (!string.IsNullOrEmpty(NationalityName))
        {
            clients = clients.Where(o => o.Nationality.Name.Contains(NationalityName ?? ""));
        }

        if (!string.IsNullOrEmpty(ZodiacSignName))
        {
            clients = clients.Where(o => o.ZodiacSign.Name.Contains(ZodiacSignName ?? ""));
        }

        if (Age.HasValue)
        {
            clients = clients.Where(o => o.PhysicalAttribute.Age == Age.Value);
        }

        if (!string.IsNullOrEmpty(Hobbies))
        {
            clients = clients.Where(o => o.PhysicalAttribute.Hobbies.Contains(Hobbies ?? ""));
        }

        switch (sortOrder)
        {
            case ClientSortState.ClientNameAsc:
                clients = clients.OrderBy(s => s.FirstName);
                break;
            case ClientSortState.ClientNameDesc:
                clients = clients.OrderByDescending(s => s.FirstName);
                break;

            case ClientSortState.GenderAsc:
                clients = clients.OrderBy(s => s.Gender);
                break;
            case ClientSortState.GenderDesc:
                clients = clients.OrderByDescending(s => s.Gender);
                break;

            case ClientSortState.NationalityNameAsc:
                clients = clients.OrderBy(s => s.Nationality.Name);
                break;
            case ClientSortState.NationalityNameDesc:
                clients = clients.OrderByDescending(s => s.Nationality.Name);
                break;

            case ClientSortState.ZodiacSignNameAsc:
                clients = clients.OrderBy(s => s.ZodiacSign.Name);
                break;
            case ClientSortState.ZodiacSignNameDesc:
                clients = clients.OrderByDescending(s => s.ZodiacSign.Name);
                break;

            case ClientSortState.AgeAsc:
                clients = clients.OrderBy(s => s.PhysicalAttribute.Age);
                break;
            case ClientSortState.AgeDesc:
                clients = clients.OrderByDescending(s => s.PhysicalAttribute.Age);
                break;

            case ClientSortState.HobbiesAsc:
                clients = clients.OrderBy(s => s.PhysicalAttribute.Hobbies);
                break;
            case ClientSortState.HobbiesDesc:
                clients = clients.OrderByDescending(s => s.PhysicalAttribute.Hobbies);
                break;

            default:
                break;
        }

        return clients;
    }
}
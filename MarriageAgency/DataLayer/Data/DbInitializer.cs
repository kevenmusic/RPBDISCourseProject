using MarriageAgency.DataLayer.Models;
using System;
using System.Linq;

namespace MarriageAgency.DataLayer.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MarriageAgencyContext db)
        {
            db.Database.EnsureCreated();

            // Проверка, есть ли клиенты в базе
            if (db.Clients.Any())
            {
                return; // База данных уже инициализирована
            }

            Random rand = new(1);

            // Вставка данных для Знаков Зодиака
            string[] zodiacSigns = { "Овен", "Телец", "Близнецы", "Рак", "Лев", "Дева", "Весы", "Скорпион", "Стрелец", "Козерог", "Водолей", "Рыбы" };
            foreach (var sign in zodiacSigns)
            {
                db.ZodiacSigns.Add(new ZodiacSign { Name = sign, Description = "Описание для " + sign });
            }
            db.SaveChanges();

            // Вставка данных для Национальностей
            string[] nationalities = { "Американец", "Француз", "Немец", "Русский", "Китаец", "Индиец", "Бразилец" };
            foreach (var nationality in nationalities)
            {
                db.Nationalities.Add(new Nationality { Name = nationality, Notes = "Гражданин " + nationality });
            }
            db.SaveChanges();

            // Массивы имен и отчеств
            string[] firstNames = { "Иван", "Мария", "Алексей", "Ольга", "Дмитрий" };
            string[] lastNames = { "Иванов", "Петров", "Сидоров", "Кузнецов", "Новиков" };
            string[] middleNames = { "Иванович", "Петрович", "Сергеевич", "Александрович", "Владимирович", "Дмитриевич", "Максимович" };

            // Вставка данных для Клиентов
            for (int i = 0; i < 50; i++)
            {
                var zodiacSignId = rand.Next(1, zodiacSigns.Length + 1);
                var nationalityId = rand.Next(1, nationalities.Length + 1);

                var client = new Client
                {
                    FirstName = firstNames[rand.Next(firstNames.Length)],
                    LastName = lastNames[rand.Next(lastNames.Length)],
                    MiddleName = middleNames[rand.Next(middleNames.Length)], // Отчество
                    Gender = rand.Next(2) == 0 ? "Мужской" : "Женский",
                    BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-rand.Next(18, 60))),
                    ZodiacSignId = zodiacSignId,
                    NationalityId = nationalityId,
                    Profession = "Профессия " + i,

                    // Инициализируем поле ClientPhoto значением
                    ClientPhoto = $"/images/clients/client{i % 10 + 1}.jpg" // Используем модуль, чтобы повторять фото
                };

                db.Clients.Add(client);
                db.SaveChanges(); // Сохранение после добавления клиента

                // Добавление Контакта для каждого клиента
                db.Contacts.Add(new Contact
                {
                    ClientId = client.ClientId, // Связываем с клиентом по ID
                    Address = "Адрес " + i,
                    Phone = "+7916123456" + i,
                    PassportData = "Паспортные данные " + i
                });

                // Добавление Физических характеристик для каждого клиента
                db.PhysicalAttributes.Add(new PhysicalAttribute
                {
                    ClientId = client.ClientId, // Связываем с клиентом по ID
                    Age = rand.Next(18, 60),
                    Height = (decimal)(150 + rand.NextDouble() * 50),
                    Weight = (decimal)(50 + rand.NextDouble() * 50),
                    ChildrenCount = rand.Next(0, 5),
                    MaritalStatus = rand.Next(2) == 0 ? "Холост" : "Женат",
                    BadHabits = rand.Next(2) == 0 ? "Нет" : "Курение",
                    Hobbies = "Хобби " + i
                });
            }
            db.SaveChanges();

            // Вставка данных для Сотрудников
            string[] employeePositions = { "Менеджер", "Консультант", "Администратор" };
            for (int i = 0; i < 20; i++)
            {
                db.Employees.Add(new Employee
                {
                    FirstName = firstNames[rand.Next(firstNames.Length)],
                    LastName = lastNames[rand.Next(lastNames.Length)],
                    MiddleName = middleNames[rand.Next(middleNames.Length)],
                    Position = employeePositions[rand.Next(employeePositions.Length)],
                    BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-rand.Next(25, 60)))
                });
            }
            db.SaveChanges();

            // Вставка данных для Дополнительных Услуг
            string[] additionalServices = { "Премиум сватовство", "Консультации", "Организация мероприятий", "Доставка подарков и цветов" };
            foreach (var serviceName in additionalServices)
            {
                db.AdditionalServices.Add(new AdditionalService
                {
                    Name = serviceName,
                    Description = "Описание услуги: " + serviceName,
                    Price = (decimal)(100 + rand.NextDouble() * 500)
                });
            }
            db.SaveChanges();

            // Вставка данных для Услуг
            for (int i = 0; i < 100; i++)
            {
                db.Services.Add(new Service
                {
                    ClientId = rand.Next(1, 51), // Используем 51, так как ID клиентов начинаются с 1
                    EmployeeId = rand.Next(1, 21), // Используем 21, так как ID сотрудников начинаются с 1
                    AdditionalServiceId = rand.Next(1, additionalServices.Length + 1),
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(-rand.Next(1, 100))),
                    Cost = (decimal)(200 + rand.NextDouble() * 1000)
                });
            }
            db.SaveChanges();
        }
    }
}

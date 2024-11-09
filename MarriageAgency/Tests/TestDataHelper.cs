using MarriageAgency.DataLayer.Models;

namespace Tests
{
    internal class TestDataHelper
    {
        public static List<Client> GetFakeClientsList()
        {
            return new List<Client>
            {
                new Client {
                    ClientId = 1,
                    FirstName = "Иван",
                    LastName = "Иванов",
                    MiddleName = "Иванович",
                    Gender = "Мужской",
                    BirthDate = new DateOnly(1990, 1, 15),
                    ZodiacSignId = 3, // Example ZodiacSignId
                    NationalityId = 1, // Example NationalityId
                    Profession = "Инженер",
                    ClientPhoto = "ivan_ivanov.jpg",
                    Contact = new Contact {
                        ClientId = 1,
                        Address = "ул. Ленина, д. 10",
                        Phone = "+7 123 456 7890",
                        PassportData = "1234 567890"
                    },
                    Nationality = new Nationality {
                        Name = "Русский"
                    },
                    ZodiacSign = new ZodiacSign {
                        Name = "Козерог"
                    },
                },
                new Client {
                    ClientId = 2,
                    FirstName = "Мария",
                    LastName = "Петрова",
                    MiddleName = "Сергеевна",
                    Gender = "Женский",
                    BirthDate = new DateOnly(1995, 5, 20),
                    ZodiacSignId = 4, // Example ZodiacSignId
                    NationalityId = 2, // Example NationalityId
                    Profession = "Врач",
                    ClientPhoto = "maria_petrovа.jpg",
                    Contact = new Contact {
                        ClientId = 2,
                        Address = "ул. Пушкина, д. 20",
                        Phone = "+7 987 654 3210",
                        PassportData = "2233 445566"
                    },
                    Nationality = new Nationality {
                        Name = "Русская"
                    },
                    ZodiacSign = new ZodiacSign {
                        Name = "Телец"
                    },
                },
                new Client {
                    ClientId = 3,
                    FirstName = "Александр",
                    LastName = "Сидоров",
                    MiddleName = "Петрович",
                    Gender = "Мужской",
                    BirthDate = new DateOnly(1987, 3, 30),
                    ZodiacSignId = 1, // Example ZodiacSignId
                    NationalityId = 1, // Example NationalityId
                    Profession = "Архитектор",
                    ClientPhoto = "aleksandr_sidorov.jpg",
                    Contact = new Contact {
                        ClientId = 3,
                        Address = "ул. Гагарина, д. 5",
                        Phone = "+7 111 222 3333",
                        PassportData = "3344 556677"
                    },
                    Nationality = new Nationality {
                        Name = "Русский"
                    },
                    ZodiacSign = new ZodiacSign {
                        Name = "Овен"
                    },
                }
            };
        }

        public static List<Employee> GetFakeEmployeesList()
        {
            return new List<Employee>
            {
                new Employee {
                    EmployeeId = 1,
                    FirstName = "Сергей",
                    LastName = "Петров",
                    MiddleName = "Иванович",
                    Position = "Менеджер",
                    BirthDate = new DateOnly(1985, 7, 10)
                },
                new Employee {
                    EmployeeId = 2,
                    FirstName = "Анна",
                    LastName = "Иванова",
                    MiddleName = "Сергеевна",
                    Position = "Врач",
                    BirthDate = new DateOnly(1990, 12, 5)
                },
                new Employee {
                    EmployeeId = 3,
                    FirstName = "Дмитрий",
                    LastName = "Смирнов",
                    MiddleName = "Алексеевич",
                    Position = "Архитектор",
                    BirthDate = new DateOnly(1983, 4, 18)
                }
            };
        }

        public static List<AdditionalService> GetFakeAdditionalServicesList()
        {
            return new List<AdditionalService>
            {
                new AdditionalService { AdditionalServiceId = 1, Name = "VIP Service" },
                new AdditionalService { AdditionalServiceId = 2, Name = "Early Check-in" },
                new AdditionalService { AdditionalServiceId = 3, Name = "Услуга3" },
            };
        }

        public static List<Service> GetFakeServicesList()
        {
            int services_number = 10; // Количество услуг
            int clients_number = GetFakeClientsList().Count; // Получение списка клиентов
            int employees_number = GetFakeEmployeesList().Count; // Получение списка сотрудников
            int additionalServices_number = GetFakeAdditionalServicesList().Count; // Получение списка дополнительных услуг

            // Проверка на пустоту
            if (clients_number == 0 || employees_number == 0 || additionalServices_number == 0)
            {
                throw new InvalidOperationException("One or more required lists are empty");
            }

            Random randObj = new Random(1); // Создание генератора случайных чисел
            List<Service> services = new List<Service>(); // Список для хранения услуг

            // Заполнение списка услуг
            for (int serviceID = 1; serviceID <= services_number; serviceID++)
            {
                int clientID = randObj.Next(1, clients_number + 1); // Случайный выбор клиента
                int employeeID = randObj.Next(1, employees_number + 1); // Случайный выбор сотрудника
                int additionalServiceID = randObj.Next(1, additionalServices_number + 1); // Случайный выбор дополнительной услуги
                DateTime today = DateTime.Now.Date; // Текущая дата
                DateOnly serviceDate = DateOnly.FromDateTime(today.AddDays(-serviceID)); // Дата услуги (от сегодняшней даты)

                // Создание новой услуги и добавление в список
                services.Add(new Service
                {
                    ServiceId = serviceID,
                    ClientId = clientID,
                    EmployeeId = employeeID,
                    AdditionalServiceId = additionalServiceID,
                    Date = serviceDate,
                    Cost = randObj.Next(50, 200), // Случайная стоимость услуги
                    Client = GetFakeClientsList().SingleOrDefault(m => m.ClientId == clientID), // Привязка клиента
                    Employee = GetFakeEmployeesList().SingleOrDefault(m => m.EmployeeId == employeeID), // Привязка сотрудника
                    AdditionalService = GetFakeAdditionalServicesList().SingleOrDefault(m => m.AdditionalServiceId == additionalServiceID) // Привязка дополнительной услуги
                });
            }
            var clients = GetFakeClientsList();
            var employees = GetFakeEmployeesList();
            var additionalServices = GetFakeAdditionalServicesList();

            Console.WriteLine($"Clients Count: {clients.Count}");
            Console.WriteLine($"Employees Count: {employees.Count}");
            Console.WriteLine($"AdditionalServices Count: {additionalServices.Count}");

            return services;
        }


    }
}

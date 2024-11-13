using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels;
using MarriageAgency.ViewModels.ClientsViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq.EntityFrameworkCore;

namespace Tests
{
    public class ClientsControllerTests
    {
        [Fact]
        public async Task GetClientList()
        {
            // Arrange
            var clients = TestDataHelper.GetFakeClientsList();
            var nationalities = TestDataHelper.GetFakeNationalitiesList();
            var zodiacSigns = TestDataHelper.GetFakeZodiacSignsList();

            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Clients).ReturnsDbSet(clients);
            marriageAgencyContextMock.Setup(x => x.Nationalities).ReturnsDbSet(nationalities);
            marriageAgencyContextMock.Setup(x => x.ZodiacSigns).ReturnsDbSet(zodiacSigns);

            // Мокирование IWebHostEnvironment
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ClientsController
            var clientsController = new ClientsController(
                marriageAgencyContextMock.Object,
                webHostEnvironmentMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await clientsController.Index(new FilterClientsViewModel(), SortState.No, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            var model = Assert.IsAssignableFrom<ClientsViewModel>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Clients.Count());
        }

        [Fact]
        public async Task GetClient()
        {
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Clients).ReturnsDbSet(clients);
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ClientsController
            var clientsController = new ClientsController(
                marriageAgencyContextMock.Object,
                webHostEnvironmentMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await clientsController.Details(4);
            var foundResult = await clientsController.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Clients).ReturnsDbSet(clients);
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ClientsController
            var clientsController = new ClientsController(
                marriageAgencyContextMock.Object,
                webHostEnvironmentMock.Object,
                configurationMock.Object
            );
            clientsController.ModelState.AddModelError("error", "some error");

            // Act
            var result = await clientsController.Create(client: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Clients).ReturnsDbSet(clients);
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ClientsController
            var clientsController = new ClientsController(
                marriageAgencyContextMock.Object,
                webHostEnvironmentMock.Object,
                configurationMock.Object
            );

            // Arrange: создание нового клиента с уникальным ClientId
            var client = new Client
            {
                ClientId = 4,
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванович",
                Gender = "Мужской",
                BirthDate = new DateOnly(1990, 1, 15),
                ZodiacSignId = 3,
                NationalityId = 1,
                Profession = "Инженер",
                ClientPhoto = "sd",
                Contact = new Contact
                {
                    ClientId = 4,
                    Address = "ул. Ленина, д. 10",
                    Phone = "+7 123 456 7890",
                    PassportData = "1234 567890"
                },
                Nationality = new Nationality
                {
                    Name = "Русский"
                },
                ZodiacSign = new ZodiacSign
                {
                    Name = "Козерог"
                },
            };

            // Act: добавление нового клиента в базу данных
            var result = await clientsController.Create(client);

            // Assert: проверка перенаправления на действие Index
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            marriageAgencyContextMock.Verify();
        }

        [Fact]
        public async Task Edit_ReturnsNotFound()
        {
            // Arrange
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Clients).ReturnsDbSet(clients);
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ClientsController
            var clientsController = new ClientsController(
                marriageAgencyContextMock.Object,
                webHostEnvironmentMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await clientsController.Edit(4);
            var foundResult = await clientsController.Edit(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Edit_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Clients).ReturnsDbSet(clients);
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ClientsController
            var clientsController = new ClientsController(
                marriageAgencyContextMock.Object,
                webHostEnvironmentMock.Object,
                configurationMock.Object
            );

            // Arrange: создание нового клиента с уникальным ClientId
            var client = new Client
            {
                ClientId = 4,
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванович",
                Gender = "Мужской",
                BirthDate = new DateOnly(1990, 1, 15),
                ZodiacSignId = 3,
                NationalityId = 1,
                Profession = "Инженер",
                ClientPhoto = "sd",
                Contact = new Contact
                {
                    ClientId = 4,
                    Address = "ул. Ленина, д. 10",
                    Phone = "+7 123 456 7890",
                    PassportData = "1234 567890"
                },
                Nationality = new Nationality
                {
                    Name = "Русский"
                },
                ZodiacSign = new ZodiacSign
                {
                    Name = "Козерог"
                },
            };
            var result = await clientsController.Edit(1, client);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Clients).ReturnsDbSet(clients);
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ClientsController
            var clientsController = new ClientsController(
                marriageAgencyContextMock.Object,
                webHostEnvironmentMock.Object,
                configurationMock.Object
            );

            // Arrange: создание нового клиента с уникальным ClientId
            var client = new Client
            {
                ClientId = 3,
                FirstName = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванович",
                Gender = "Мужской",
                BirthDate = new DateOnly(1990, 1, 15),
                ZodiacSignId = 3,
                NationalityId = 1,
                Profession = "Инженер",
                ClientPhoto = "sd",
                Contact = new Contact
                {
                    ClientId = 3,
                    Address = "ул. Ленина, д. 10",
                    Phone = "+7 123 456 7890",
                    PassportData = "1234 567890"
                },
                Nationality = new Nationality
                {
                    Name = "Русский"
                },
                ZodiacSign = new ZodiacSign
                {
                    Name = "Козерог"
                },
            };
            var result = await clientsController.Edit(3, client);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            marriageAgencyContextMock.Verify();
        }

        [Fact]
        public async Task Delete_ReturnsNotFound()
        {
            // Arrange
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Clients).ReturnsDbSet(clients);
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ClientsController
            var clientsController = new ClientsController(
                marriageAgencyContextMock.Object,
                webHostEnvironmentMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await clientsController.Delete(4);
            var foundResult = await clientsController.Delete(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Delete_ReturnsARedirectAndDelete()
        {
            // Arrange
            var clients = TestDataHelper.GetFakeClientsList();
            var contacts = TestDataHelper.GetFakeContactsList();
            var physicalAttributes = TestDataHelper.GetFakePhysicalAttributesList();
            var nationalities = TestDataHelper.GetFakeNationalitiesList();
            var zodiacSigns = TestDataHelper.GetFakeZodiacSignsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Clients).ReturnsDbSet(clients);
            marriageAgencyContextMock.Setup(x => x.Contacts).ReturnsDbSet(contacts);
            marriageAgencyContextMock.Setup(x => x.PhysicalAttributes).ReturnsDbSet(physicalAttributes);
            marriageAgencyContextMock.Setup(x => x.Nationalities).ReturnsDbSet(nationalities);
            marriageAgencyContextMock.Setup(x => x.ZodiacSigns).ReturnsDbSet(zodiacSigns);
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ClientsController
            var clientsController = new ClientsController(
                marriageAgencyContextMock.Object,
                webHostEnvironmentMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await clientsController.DeleteConfirmed(3);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            marriageAgencyContextMock.Verify();
        }
    }
}

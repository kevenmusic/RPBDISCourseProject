using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels.ContactsViewModel;
using MarriageAgency.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq.EntityFrameworkCore;
using MarriageAgency.ViewModels.AdditionalServicesViewModel;

namespace Tests
{
    public class ContactsControllerTests
    {
        [Fact]
        public async Task GetContactsList()
        {
            // Arrange
            var contacts = TestDataHelper.GetFakeContactsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Contacts).ReturnsDbSet(contacts);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ContactsController
            var contactsController = new ContactsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await contactsController.Index(new FilterContactsViewModel(), SortState.No, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            var model = Assert.IsAssignableFrom<ContactsViewModel>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Contacts.Count());
        }

        [Fact]
        public async Task GetContact()
        {
            // Arrange
            var contacts = TestDataHelper.GetFakeContactsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Contacts).ReturnsDbSet(contacts);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ContactsController
            var contactsController = new ContactsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await contactsController.Details(4);
            var foundResult = await contactsController.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var contacts = TestDataHelper.GetFakeContactsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Contacts).ReturnsDbSet(contacts);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ContactsController
            var contactsController = new ContactsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );
            contactsController.ModelState.AddModelError("error", "some error");

            // Act
            var result = await contactsController.Create(contact: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var contacts = TestDataHelper.GetFakeContactsList();
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Contacts).ReturnsDbSet(contacts);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ContactsController
            var contactsController = new ContactsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var contact = new Contact
            {
                ClientId = 3,
                Address = "ул. Ленина, д. 10",
                Phone = "+7 123 456 7890",
                PassportData = "2 35252525",
                Client = clients.SingleOrDefault(c => c.ClientId == 3)
            };

            // Act
            var result = await contactsController.Create(contact);

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
            var contacts = TestDataHelper.GetFakeContactsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Contacts).ReturnsDbSet(contacts);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ContactsController
            var contactsController = new ContactsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await contactsController.Edit(4);
            var foundResult = await contactsController.Edit(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Edit_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var contacts = TestDataHelper.GetFakeContactsList();
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Contacts).ReturnsDbSet(contacts);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ContactsController
            var contactsController = new ContactsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var contact = new Contact
            {
                ClientId = 3,
                Address = "ул. Ленина, д. 10",
                Phone = "+7 123 456 7890",
                PassportData = "2 35252525",
                Client = clients.SingleOrDefault(c => c.ClientId == 3)
            };

            var result = await contactsController.Edit(1, contact);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var contacts = TestDataHelper.GetFakeContactsList();
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Contacts).ReturnsDbSet(contacts);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ContactsController
            var contactsController = new ContactsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var contact = new Contact
            {
                ClientId = 3,
                Address = "ул. Ленина, д. 10",
                Phone = "+7 123 456 7890",
                PassportData = "2 35252525",
                Client = clients.SingleOrDefault(c => c.ClientId == 3)
            };

            var result = await contactsController.Edit(3, contact);

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
            var contacts = TestDataHelper.GetFakeContactsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Contacts).ReturnsDbSet(contacts);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ContactsController
            var contactsController = new ContactsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await contactsController.Delete(4);
            var foundResult = await contactsController.Delete(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Delete_ReturnsARedirectAndDelete()
        {
            // Arrange
            var contacts = TestDataHelper.GetFakeContactsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Contacts).ReturnsDbSet(contacts);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ContactsController
            var contactsController = new ContactsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await contactsController.DeleteConfirmed(3);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            marriageAgencyContextMock.Verify();
        }
    }
}

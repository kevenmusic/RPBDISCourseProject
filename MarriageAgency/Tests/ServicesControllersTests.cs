using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels;
using MarriageAgency.ViewModels.ServicesViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq.EntityFrameworkCore;

namespace Tests
{
    public class ServicesControllersTests
    {
        [Fact]
        public async Task GetServicesList()
        {
            // Arrange
            var services = TestDataHelper.GetFakeServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Services).ReturnsDbSet(services);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ServicesController
            var servicesController = new ServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await servicesController.Index(new FilterServicesViewModel(), SortState.No, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            var model = Assert.IsAssignableFrom<ServicesViewModel>(viewResult.ViewData.Model);
            Assert.Equal(TestDataHelper.GetFakeServicesList().Count, model.Services.Count());
            Assert.NotNull(model.Services);
        }

        [Fact]
        public async Task GetService()
        {
            // Arrange
            var services = TestDataHelper.GetFakeServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Services).ReturnsDbSet(services);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ServicesController
            var servicesController = new ServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await servicesController.Details(11);
            var foundResult = await servicesController.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var services = TestDataHelper.GetFakeServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Services).ReturnsDbSet(services);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ServicesController
            var servicesController = new ServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            servicesController.ModelState.AddModelError("error", "some error");

            // Act
            var result = await servicesController.Create(service: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var services = TestDataHelper.GetFakeServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Services).ReturnsDbSet(services);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ServicesController
            var servicesController = new ServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange: создание новой услуги
            var service = new Service
            {
                ServiceId = 4,
                AdditionalServiceId = 2,
                ClientId = 3,
                EmployeeId = 3,
                Date = new DateOnly(2024, 11, 9),
                Cost = 200.50m,
                AdditionalService = TestDataHelper.GetFakeAdditionalServicesList().SingleOrDefault(m => m.AdditionalServiceId == 2),
                Client = TestDataHelper.GetFakeClientsList().SingleOrDefault(m => m.ClientId == 3),
                Employee = TestDataHelper.GetFakeEmployeesList().SingleOrDefault(m => m.EmployeeId == 3),
            };


            // Act: добавление новой услуги в базу данных
            var result = await servicesController.Create(service);

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
            var services = TestDataHelper.GetFakeServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Services).ReturnsDbSet(services);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ServicesController
            var servicesController = new ServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await servicesController.Edit(400);
            var foundResult = await servicesController.Edit(5);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Edit_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var services = TestDataHelper.GetFakeServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Services).ReturnsDbSet(services);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ServicesController
            var servicesController = new ServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange: создание новой услуги
            var service = new Service
            {
                ServiceId = 4,
                AdditionalServiceId = 2,
                ClientId = 3,
                EmployeeId = 5,
                Date = new DateOnly(2024, 11, 9),
                Cost = 200.50m,
                AdditionalService = TestDataHelper.GetFakeAdditionalServicesList().SingleOrDefault(m => m.AdditionalServiceId == 2),
                Client = TestDataHelper.GetFakeClientsList().SingleOrDefault(m => m.ClientId == 3),
                Employee = TestDataHelper.GetFakeEmployeesList().SingleOrDefault(m => m.EmployeeId == 5),
            };

            var result = await servicesController.Edit(1, service);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var services = TestDataHelper.GetFakeServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Services).ReturnsDbSet(services);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ServicesController
            var servicesController = new ServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange: создание новой услуги
            var service = new Service
            {
                ServiceId = 3,
                AdditionalServiceId = 2,
                ClientId = 3,
                EmployeeId = 3,
                Date = new DateOnly(2024, 11, 9),
                Cost = 200.50m,
                AdditionalService = TestDataHelper.GetFakeAdditionalServicesList().SingleOrDefault(m => m.AdditionalServiceId == 2),
                Client = TestDataHelper.GetFakeClientsList().SingleOrDefault(m => m.ClientId == 3),
                Employee = TestDataHelper.GetFakeEmployeesList().SingleOrDefault(m => m.EmployeeId == 3)
            };

            var result = await servicesController.Edit(3, service);

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
            var services = TestDataHelper.GetFakeServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Services).ReturnsDbSet(services);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ServicesController
            var servicesController = new ServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await servicesController.Delete(11);
            var foundResult = await servicesController.Delete(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Delete_ReturnsARedirectAndDelete()
        {
            // Arrange
            var services = TestDataHelper.GetFakeServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Services).ReturnsDbSet(services);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ServicesController
            var servicesController = new ServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await servicesController.DeleteConfirmed(3);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            marriageAgencyContextMock.Verify();
        }
    }
}

using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels.AdditionalServicesViewModel;
using MarriageAgency.ViewModels.SortStates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq.EntityFrameworkCore;

namespace Tests
{
    public class AdditionalServicesControllerTests
    {
        [Fact]
        public async Task GetAdditionalServicesList()
        {
            // Arrange
            var additionalServices = TestDataHelper.GetFakeAdditionalServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.AdditionalServices).ReturnsDbSet(additionalServices);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация AdditionalServicesController
            var additionalServicesController = new AdditionalServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await additionalServicesController.Index(new FilterAdditionalServicesViewModel(), AdditionalServiceSortState.No, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            var model = Assert.IsAssignableFrom<AdditionalServicesViewModel>(viewResult.ViewData.Model);
            Assert.Equal(5, model.AdditionalServices.Count());
        }

        [Fact]
        public async Task GetAdditionalService()
        {
            var additionalServices = TestDataHelper.GetFakeAdditionalServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.AdditionalServices).ReturnsDbSet(additionalServices);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация AdditionalServicesController
            var additionalServicesController = new AdditionalServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await additionalServicesController.Details(6);
            var foundResult = await additionalServicesController.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            var additionalServices = TestDataHelper.GetFakeAdditionalServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.AdditionalServices).ReturnsDbSet(additionalServices);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация AdditionalServicesController
            var additionalServicesController = new AdditionalServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );
            additionalServicesController.ModelState.AddModelError("error", "some error");

            // Act
            var result = await additionalServicesController.Create(additionalService: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            var additionalServices = TestDataHelper.GetFakeAdditionalServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.AdditionalServices).ReturnsDbSet(additionalServices);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация AdditionalServicesController
            var additionalServicesController = new AdditionalServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var additionalService = new AdditionalService
            {
                AdditionalServiceId = 6,
                Name = "Guided Tour",
                Description = "Экскурсионное обслуживание с гидом по интересным местам.",
                Price = 520.00M,
            };

            // Act
            var result = await additionalServicesController.Create(additionalService);

            // Assert: проверка перенаправления на действие Index
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            marriageAgencyContextMock.Verify();
        }

        [Fact]
        public async Task Edit_ReturnsNotFound()
        {
            var additionalServices = TestDataHelper.GetFakeAdditionalServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.AdditionalServices).ReturnsDbSet(additionalServices);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация AdditionalServicesController
            var additionalServicesController = new AdditionalServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await additionalServicesController.Edit(6);
            var foundResult = await additionalServicesController.Edit(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Edit_ReturnsBadRequest_GivenInvalidModel()
        {
            var additionalServices = TestDataHelper.GetFakeAdditionalServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.AdditionalServices).ReturnsDbSet(additionalServices);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация AdditionalServicesController
            var additionalServicesController = new AdditionalServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var additionalService = new AdditionalService
            {
                AdditionalServiceId = 6,
                Name = "Guided Tour",
                Description = "Экскурсионное обслуживание с гидом по интересным местам.",
                Price = 520.00M,
            };
            var result = await additionalServicesController.Edit(1, additionalService);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            var additionalServices = TestDataHelper.GetFakeAdditionalServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.AdditionalServices).ReturnsDbSet(additionalServices);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация AdditionalServicesController
            var additionalServicesController = new AdditionalServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var additionalService = new AdditionalService
            {
                AdditionalServiceId = 6,
                Name = "Guided Tour",
                Description = "Экскурсионное обслуживание с гидом по интересным местам.",
                Price = 520.00M,
            };
            var result = await additionalServicesController.Edit(6, additionalService);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            marriageAgencyContextMock.Verify();
        }

        [Fact]
        public async Task Delete_ReturnsNotFound()
        {
            var additionalServices = TestDataHelper.GetFakeAdditionalServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.AdditionalServices).ReturnsDbSet(additionalServices);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация AdditionalServicesController
            var additionalServicesController = new AdditionalServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await additionalServicesController.Delete(6);
            var foundResult = await additionalServicesController.Delete(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Delete_ReturnsARedirectAndDelete()
        {
            var additionalServices = TestDataHelper.GetFakeAdditionalServicesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.AdditionalServices).ReturnsDbSet(additionalServices);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация AdditionalServicesController
            var additionalServicesController = new AdditionalServicesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await additionalServicesController.DeleteConfirmed(3);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            marriageAgencyContextMock.Verify();
        }
    }
}

using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels;
using MarriageAgency.ViewModels.AdditionalServicesViewModel;
using MarriageAgency.ViewModels.NationalitiesViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Configuration;
using Moq.EntityFrameworkCore;

namespace Tests
{
    public class NationalitiesControllerTests
    {
        [Fact]
        public async Task GetNationalitiesList()
        {
            // Arrange
            var nationalities = TestDataHelper.GetFakeNationalitiesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Nationalities).ReturnsDbSet(nationalities);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация NationalitiesController
            var nationalitiesController = new NationalitiesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await nationalitiesController.Index(new FilterNationalitiesViewModel(), SortState.No, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            var model = Assert.IsAssignableFrom<NationalitiesViewModel>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Nationalities.Count());
        }

        [Fact]
        public async Task GetNationality()
        {
            // Arrange
            var nationalities = TestDataHelper.GetFakeNationalitiesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Nationalities).ReturnsDbSet(nationalities);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация NationalitiesController
            var nationalitiesController = new NationalitiesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await nationalitiesController.Details(4);
            var foundResult = await nationalitiesController.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var nationalities = TestDataHelper.GetFakeNationalitiesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Nationalities).ReturnsDbSet(nationalities);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация NationalitiesController
            var nationalitiesController = new NationalitiesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );
            nationalitiesController.ModelState.AddModelError("error", "some error");

            // Act
            var result = await nationalitiesController.Create(nationality: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var nationalities = TestDataHelper.GetFakeNationalitiesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Nationalities).ReturnsDbSet(nationalities);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация NationalitiesController
            var nationalitiesController = new NationalitiesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var nationality = new Nationality
            {
                NationalityId = 4,
                Name = "Казах",
                Notes = "Примечание 4"
            };

            // Act
            var result = await nationalitiesController.Create(nationality);

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
            var nationalities = TestDataHelper.GetFakeNationalitiesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Nationalities).ReturnsDbSet(nationalities);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация NationalitiesController
            var nationalitiesController = new NationalitiesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await nationalitiesController.Edit(4);
            var foundResult = await nationalitiesController.Edit(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Edit_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var nationalities = TestDataHelper.GetFakeNationalitiesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Nationalities).ReturnsDbSet(nationalities);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация NationalitiesController
            var nationalitiesController = new NationalitiesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var nationality = new Nationality
            {
                NationalityId = 4,
                Name = "Казах",
                Notes = "Примечание 4"
            };
            var result = await nationalitiesController.Edit(1, nationality);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var nationalities = TestDataHelper.GetFakeNationalitiesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Nationalities).ReturnsDbSet(nationalities);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация NationalitiesController
            var nationalitiesController = new NationalitiesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var nationality = new Nationality
            {
                NationalityId = 4,
                Name = "Казах",
                Notes = "Примечание 4"
            };

            var result = await nationalitiesController.Edit(4, nationality);

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
            var nationalities = TestDataHelper.GetFakeNationalitiesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Nationalities).ReturnsDbSet(nationalities);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация NationalitiesController
            var nationalitiesController = new NationalitiesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await nationalitiesController.Delete(4);
            var foundResult = await nationalitiesController.Delete(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Delete_ReturnsARedirectAndDelete()
        {
            // Arrange
            var nationalities = TestDataHelper.GetFakeNationalitiesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Nationalities).ReturnsDbSet(nationalities);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация NationalitiesController
            var nationalitiesController = new NationalitiesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await nationalitiesController.DeleteConfirmed(3);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            marriageAgencyContextMock.Verify();
        }
    }
}

using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels;
using MarriageAgency.ViewModels.ZodiacSignsViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq.EntityFrameworkCore;

namespace Tests
{
    public class ZodiacSignsControllerTests
    {
        [Fact]
        public async Task GetZodiacSignsList()
        {
            // Arrange
            var zodiacSigns = TestDataHelper.GetFakeZodiacSignsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.ZodiacSigns).ReturnsDbSet(zodiacSigns);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ZodiacSignsController
            var zodiacSignsController = new ZodiacSignsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await zodiacSignsController.Index(new FilterZodiacSignsViewModel(), SortState.No, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            var model = Assert.IsAssignableFrom<ZodiacSignsViewModel>(viewResult.ViewData.Model);
            Assert.Equal(4, model.ZodiacSigns.Count());
        }

        [Fact]
        public async Task GetZodiacSign()
        {
            // Arrange
            var zodiacSigns = TestDataHelper.GetFakeZodiacSignsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.ZodiacSigns).ReturnsDbSet(zodiacSigns);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ZodiacSignsController
            var zodiacSignsController = new ZodiacSignsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await zodiacSignsController.Details(5);
            var foundResult = await zodiacSignsController.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var zodiacSigns = TestDataHelper.GetFakeZodiacSignsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.ZodiacSigns).ReturnsDbSet(zodiacSigns);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ZodiacSignsController
            var zodiacSignsController = new ZodiacSignsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );
            zodiacSignsController.ModelState.AddModelError("error", "some error");

            // Act
            var result = await zodiacSignsController.Create(zogiacSign: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var zodiacSigns = TestDataHelper.GetFakeZodiacSignsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.ZodiacSigns).ReturnsDbSet(zodiacSigns);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ZodiacSignsController
            var zodiacSignsController = new ZodiacSignsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var zodiacSign = new ZodiacSign
            {
                ZodiacSignId = 4,
                Name = "Весы",
                Description = "Весы — единственный неодушевлённый предмет в знаках зодиака.",
            };

            // Act
            var result = await zodiacSignsController.Create(zodiacSign);

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
            var zodiacSigns = TestDataHelper.GetFakeZodiacSignsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.ZodiacSigns).ReturnsDbSet(zodiacSigns);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ZodiacSignsController
            var zodiacSignsController = new ZodiacSignsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await zodiacSignsController.Edit(5);
            var foundResult = await zodiacSignsController.Edit(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Edit_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var zodiacSigns = TestDataHelper.GetFakeZodiacSignsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.ZodiacSigns).ReturnsDbSet(zodiacSigns);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ZodiacSignsController
            var zodiacSignsController = new ZodiacSignsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var zodiacSign = new ZodiacSign
            {
                ZodiacSignId = 4,
                Name = "Весы",
                Description = "Весы — единственный неодушевлённый предмет в знаках зодиака.",
            };

            var result = await zodiacSignsController.Edit(1, zodiacSign);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var zodiacSigns = TestDataHelper.GetFakeZodiacSignsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.ZodiacSigns).ReturnsDbSet(zodiacSigns);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ZodiacSignsController
            var zodiacSignsController = new ZodiacSignsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var zodiacSign = new ZodiacSign
            {
                ZodiacSignId = 4,
                Name = "Весы",
                Description = "Весы — единственный неодушевлённый предмет в знаках зодиака.",
            };
            var result = await zodiacSignsController.Edit(4, zodiacSign);

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
            var zodiacSigns = TestDataHelper.GetFakeZodiacSignsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.ZodiacSigns).ReturnsDbSet(zodiacSigns);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ZodiacSignsController
            var zodiacSignsController = new ZodiacSignsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await zodiacSignsController.Delete(5);
            var foundResult = await zodiacSignsController.Delete(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Delete_ReturnsARedirectAndDelete()
        {
            // Arrange
            var zodiacSigns = TestDataHelper.GetFakeZodiacSignsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.ZodiacSigns).ReturnsDbSet(zodiacSigns);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация ZodiacSignsController
            var zodiacSignsController = new ZodiacSignsController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await zodiacSignsController.DeleteConfirmed(3);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            marriageAgencyContextMock.Verify();
        }
    }
}

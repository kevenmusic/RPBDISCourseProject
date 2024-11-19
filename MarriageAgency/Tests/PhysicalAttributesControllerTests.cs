using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels;
using MarriageAgency.ViewModels.AdditionalServicesViewModel;
using MarriageAgency.ViewModels.PhysicalAttributesViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Configuration;
using Moq.EntityFrameworkCore;
using MarriageAgency.ViewModels.SortStates;

namespace Tests
{
    public class PhysicalAttributesControllerTests
    {
        [Fact]
        public async Task GetPhysicalAttributesList()
        {
            // Arrange
            var physicalAttributes = TestDataHelper.GetFakePhysicalAttributesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.PhysicalAttributes).ReturnsDbSet(physicalAttributes);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация PhysicalAttributesController
            var physicalAttributesController = new PhysicalAttributesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await physicalAttributesController.Index(new FilterPhysicalAttributesViewModel(), PhysicalAttributeSortState.No, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            var model = Assert.IsAssignableFrom<PhysicalAttributesViewModel>(viewResult.ViewData.Model);
            Assert.Equal(3, model.PhysicalAttributes.Count());
        }

        [Fact]
        public async Task GetPhysicalAttribute()
        {
            // Arrange
            var physicalAttributes = TestDataHelper.GetFakePhysicalAttributesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.PhysicalAttributes).ReturnsDbSet(physicalAttributes);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация PhysicalAttributesController
            var physicalAttributesController = new PhysicalAttributesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await physicalAttributesController.Details(4);
            var foundResult = await physicalAttributesController.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var physicalAttributes = TestDataHelper.GetFakePhysicalAttributesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.PhysicalAttributes).ReturnsDbSet(physicalAttributes);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация PhysicalAttributesController
            var physicalAttributesController = new PhysicalAttributesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );
            physicalAttributesController.ModelState.AddModelError("error", "some error");

            // Act
            var result = await physicalAttributesController.Create(physicalAttribute: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var physicalAttributes = TestDataHelper.GetFakePhysicalAttributesList();
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.PhysicalAttributes).ReturnsDbSet(physicalAttributes);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация PhysicalAttributesController
            var physicalAttributesController = new PhysicalAttributesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var physicalAttribute = new PhysicalAttribute
            {
                ClientId = 3,
                Age = 52,
                Height = 195.0M,
                Weight = 85.0M,
                ChildrenCount = 0,
                MaritalStatus = "Холост",
                BadHabits = "Алкоголь",
                Hobbies = "Велоспорт, игры на гитаре",
                Client = clients.SingleOrDefault(c => c.ClientId == 3)
            };

            // Act
            var result = await physicalAttributesController.Create(physicalAttribute);

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
            var physicalAttributes = TestDataHelper.GetFakePhysicalAttributesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.PhysicalAttributes).ReturnsDbSet(physicalAttributes);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация PhysicalAttributesController
            var physicalAttributesController = new PhysicalAttributesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await physicalAttributesController.Edit(4);
            var foundResult = await physicalAttributesController.Edit(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Edit_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var physicalAttributes = TestDataHelper.GetFakePhysicalAttributesList();
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.PhysicalAttributes).ReturnsDbSet(physicalAttributes);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация PhysicalAttributesController
            var physicalAttributesController = new PhysicalAttributesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var physicalAttribute = new PhysicalAttribute
            {
                ClientId = 3,
                Age = 52,
                Height = 195.0M,
                Weight = 85.0M,
                ChildrenCount = 0,
                MaritalStatus = "Холост",
                BadHabits = "Алкоголь",
                Hobbies = "Велоспорт, игры на гитаре",
                Client = clients.SingleOrDefault(c => c.ClientId == 3)
            };

            var result = await physicalAttributesController.Edit(1, physicalAttribute);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var physicalAttributes = TestDataHelper.GetFakePhysicalAttributesList();
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.PhysicalAttributes).ReturnsDbSet(physicalAttributes);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация PhysicalAttributesController
            var physicalAttributesController = new PhysicalAttributesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange
            var physicalAttribute = new PhysicalAttribute
            {
                ClientId = 3,
                Age = 52,
                Height = 195.0M,
                Weight = 85.0M,
                ChildrenCount = 0,
                MaritalStatus = "Холост",
                BadHabits = "Алкоголь",
                Hobbies = "Велоспорт, игры на гитаре",
                Client = clients.SingleOrDefault(c => c.ClientId == 3)
            };

            var result = await physicalAttributesController.Edit(3, physicalAttribute);

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
            var physicalAttributes = TestDataHelper.GetFakePhysicalAttributesList();
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.PhysicalAttributes).ReturnsDbSet(physicalAttributes);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация PhysicalAttributesController
            var physicalAttributesController = new PhysicalAttributesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await physicalAttributesController.Delete(4);
            var foundResult = await physicalAttributesController.Delete(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Delete_ReturnsARedirectAndDelete()
        {
            // Arrange
            var physicalAttributes = TestDataHelper.GetFakePhysicalAttributesList();
            var clients = TestDataHelper.GetFakeClientsList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.PhysicalAttributes).ReturnsDbSet(physicalAttributes);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация PhysicalAttributesController
            var physicalAttributesController = new PhysicalAttributesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await physicalAttributesController.DeleteConfirmed(3);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            marriageAgencyContextMock.Verify();
        }
    }
}

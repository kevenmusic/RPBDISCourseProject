using MarriageAgency.DataLayer.Data;
using MarriageAgency.DataLayer.Models;
using MarriageAgency.ViewModels;
using MarriageAgency.ViewModels.EmployeesViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Extensions.Configuration;
using Moq.EntityFrameworkCore;

namespace Tests
{
    public class EmployeesControllerTests
    {
        [Fact]
        public async Task GetEmployeeList()
        {
            // Arrange
            var employees = TestDataHelper.GetFakeEmployeesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Employees).ReturnsDbSet(employees);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация EmployeesController
            var employeesController = new EmployeesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await employeesController.Index(new FilterEmployeesViewModel(), SortState.No, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            var model = Assert.IsAssignableFrom<EmployeesViewModel>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Employees.Count());
        }

        [Fact]
        public async Task GetEmployee()
        {
            // Arrange
            var employees = TestDataHelper.GetFakeEmployeesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Employees).ReturnsDbSet(employees);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация EmployeesController
            var employeesController = new EmployeesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await employeesController.Details(4);
            var foundResult = await employeesController.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var employees = TestDataHelper.GetFakeEmployeesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Employees).ReturnsDbSet(employees);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация EmployeesController
            var employeesController = new EmployeesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );
            employeesController.ModelState.AddModelError("error", "some error");

            // Act
            var result = await employeesController.Create(employee: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {

            // Arrange
            var employees = TestDataHelper.GetFakeEmployeesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Employees).ReturnsDbSet(employees);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация EmployeesController
            var employeesController = new EmployeesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange: создание нового сотрудника
            var employee = new Employee
            {
                EmployeeId = 4,
                FirstName = "Анна",
                LastName = "Иванова",
                MiddleName = "Михайловна",
                Position = "Врач",
                BirthDate = new DateOnly(1990, 12, 5),
            };

            // Act: добавление нового сотрудника в базу данных
            var result = await employeesController.Create(employee);

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
            var employees = TestDataHelper.GetFakeEmployeesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Employees).ReturnsDbSet(employees);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация EmployeesController
            var employeesController = new EmployeesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await employeesController.Edit(4);
            var foundResult = await employeesController.Edit(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Edit_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var employees = TestDataHelper.GetFakeEmployeesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Employees).ReturnsDbSet(employees);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация EmployeesController
            var employeesController = new EmployeesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange: создание нового сотрудника
            var employee = new Employee
            {
                EmployeeId = 4,
                FirstName = "Анна",
                LastName = "Иванова",
                MiddleName = "Михайловна",
                Position = "Врач",
                BirthDate = new DateOnly(1990, 12, 5),
            };
            var result = await employeesController.Edit(1, employee);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var employees = TestDataHelper.GetFakeEmployeesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Employees).ReturnsDbSet(employees);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация EmployeesController
            var employeesController = new EmployeesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Arrange: создание нового сотрудника
            var employee = new Employee
            {
                EmployeeId = 3,
                FirstName = "Анна",
                LastName = "Иванова",
                MiddleName = "Михайловна",
                Position = "Врач",
                BirthDate = new DateOnly(1990, 12, 5),
            };
            var result = await employeesController.Edit(3, employee);

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
            var employees = TestDataHelper.GetFakeEmployeesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Employees).ReturnsDbSet(employees);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация EmployeesController
            var employeesController = new EmployeesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var notFoundResult = await employeesController.Delete(4);
            var foundResult = await employeesController.Delete(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Delete_ReturnsARedirectAndDelete()
        {
            // Arrange
            var employees = TestDataHelper.GetFakeEmployeesList();
            var marriageAgencyContextMock = new Mock<MarriageAgencyContext>();
            marriageAgencyContextMock.Setup(x => x.Employees).ReturnsDbSet(employees);

            // Мокирование IConfiguration и установка значения PageSize
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(config => config["Parameters:PageSize"]).Returns("10");

            // Инициализация EmployeesController
            var employeesController = new EmployeesController(
                marriageAgencyContextMock.Object,
                configurationMock.Object
            );

            // Act
            var result = await employeesController.DeleteConfirmed(3);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            marriageAgencyContextMock.Verify();
        }
    }
}

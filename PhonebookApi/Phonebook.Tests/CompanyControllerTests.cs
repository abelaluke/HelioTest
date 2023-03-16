using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Phonebook.Data.Services;
using Phonebook.Tests.Helpers;
using Phonebook.Web;
using Phonebook.Web.Controllers;
using Phonebook.Web.Models;

namespace Phonebook.Tests
{
    public class CompanyControllerTests
    {
        private static readonly MappingProfile MapperProfile = new();
        private static readonly MapperConfiguration Configuration = new(cfg => cfg.AddProfile(MapperProfile));
        private Mapper _mapper = new(Configuration);
        
        [Test]
        public async Task GetAllCompanies_ShouldReturnAllCompanies()
        {
            var mockService = new Mock<ICompanyService>();
            mockService.Setup(x => x.GetAllCompanies()).ReturnsAsync(TestDataHelper.GetCompanies());

            var controller = new CompanyController(mockService.Object, _mapper);
            var response = await controller.GetAllCompanies();

            var contentResult = response as OkObjectResult;
            
            Assert.IsNotNull(contentResult);
            var result = contentResult.Value as List<CompanyDto>;
            Assert.IsNotNull(result);
            Assert.That(result, Is.EquivalentTo(TestDataHelper.GetCompaniesResult()));
        }
        
        [Test]
        public async Task GetPersonCountByCompany_WhenCompanyExists_ShouldReturnCorrectValueInt()
        {
            var mockService = new Mock<ICompanyService>();
            mockService.Setup(x => x.GetPersonCountByCompany(It.IsAny<string>())).ReturnsAsync(It.IsAny<int>());

            var controller = new CompanyController(mockService.Object, _mapper);
            var response = await controller.GetPersonCountByCompany("Test");

            var contentResult = response as OkObjectResult;
            
            Assert.IsNotNull(contentResult);
            var result = contentResult.Value is int value ? value : 0;
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<int>(result);
        }
        
        [Test]
        public async Task GetPersonCountByCompany_WhenCompanyDoesNotExist_ShouldReturnBadRequest()
        {
            var mockService = new Mock<ICompanyService>();
            mockService.Setup(x => x.GetPersonCountByCompany(It.IsAny<string>())).ReturnsAsync((int?) null);
        
            var controller = new CompanyController(mockService.Object, _mapper);
            var response = await controller.GetPersonCountByCompany("Test");
        
            var contentResult = response as BadRequestObjectResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(400, contentResult.StatusCode);
            Assert.AreEqual("Company with name: Test does not exist!",contentResult.Value.ToString());
        }
        
        [Test]
        public async Task AddCompany_WhenCompanyNameIsEmptyString_ShouldReturnBadRequest()
        {
            var mockService = new Mock<ICompanyService>();
        
            var controller = new CompanyController(mockService.Object, _mapper);
            var response = await controller.AddCompany(TestDataHelper.PostFailCompanyName());
        
            var contentResult = response as BadRequestObjectResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(400, contentResult.StatusCode);
            Assert.AreEqual("Company Name cannot be an empty string",contentResult.Value.ToString());
        }
        
        [Test]
        public async Task AddCompany_WhenRegistrationDateIsGreaterThanToday_ShouldReturnBadRequest()
        {
            var mockService = new Mock<ICompanyService>();
        
            var controller = new CompanyController(mockService.Object, _mapper);
            var response = await controller.AddCompany(TestDataHelper.PostFailRegistrationDate());
        
            var contentResult = response as BadRequestObjectResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(400, contentResult.StatusCode);
            Assert.AreEqual("Registration Date cannot be in the future",contentResult.Value.ToString());
        }
        
        [Test]
        public async Task AddCompany_WhenCompanyAlreadyExists_ShouldReturnConflict()
        {
            var mockService = new Mock<ICompanyService>();
            mockService.Setup(x => x.CompanyExists(It.IsAny<string>())).ReturnsAsync(true);
        
            var controller = new CompanyController(mockService.Object, _mapper);
            var response = await controller.AddCompany(TestDataHelper.PostCompany());
        
            var contentResult = response as ConflictObjectResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(409, contentResult.StatusCode);
            Assert.AreEqual("Company with name: TestCompany already exists!",contentResult.Value.ToString());
        }
        
        [Test]
        public async Task AddCompany_WhenCompanyDoesNotExist_ShouldReturnOk()
        {
            var mockService = new Mock<ICompanyService>();
            mockService.Setup(x => x.CompanyExists(It.IsAny<string>())).ReturnsAsync(false);
        
            var controller = new CompanyController(mockService.Object, _mapper);
            var response = await controller.AddCompany(TestDataHelper.PostCompany());
        
            var contentResult = response as OkResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(200, contentResult.StatusCode);
        }
    }
}
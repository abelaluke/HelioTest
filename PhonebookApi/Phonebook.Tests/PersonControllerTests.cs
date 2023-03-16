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
    public class PersonControllerTests
    {
        private static readonly MappingProfile MapperProfile = new();
        private static readonly MapperConfiguration Configuration = new(cfg => cfg.AddProfile(MapperProfile));
        private Mapper _mapper = new(Configuration);
        
        [Test]
        public async Task GetAllPersons_ShouldReturnAllPersons()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService.Setup(x => x.GetAllPersons()).ReturnsAsync(TestDataHelper.GetPersons());
            var mockCompanyService = new Mock<ICompanyService>();

            var controller = new PersonController(mockPersonService.Object, mockCompanyService.Object, _mapper);
            var response = await controller.GetAllPersons();

            var contentResult = response as OkObjectResult;
            
            Assert.IsNotNull(contentResult);
            var result = contentResult.Value as List<PersonDto>;
            Assert.IsNotNull(result);
            Assert.That(result, Is.EquivalentTo(TestDataHelper.GetPersonsResult()));
        }
        
        [Test]
        public async Task SearchPersons_WhenEmptyStringIsPassed_ShouldReturnBadRequest()
        {
            var mockPersonService = new Mock<IPersonService>();
            var mockCompanyService = new Mock<ICompanyService>();
        
            var controller = new PersonController(mockPersonService.Object, mockCompanyService.Object, _mapper);
            var response = await controller.SearchPersons("");
        
            var contentResult = response as BadRequestObjectResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(400, contentResult.StatusCode);
            Assert.AreEqual("Search term cannot be an empty string",contentResult.Value.ToString());
        }
        
        [Test]
        public async Task SearchPersons_WhenValidStringIsPassed_ShouldReturnListIfAnyMatch()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService.Setup(x => x.SearchPersons(It.IsAny<string>())).ReturnsAsync(TestDataHelper.GetPersons());
            var mockCompanyService = new Mock<ICompanyService>();
        
            var controller = new PersonController(mockPersonService.Object, mockCompanyService.Object, _mapper);
            var response = await controller.SearchPersons("Test");
        
            var contentResult = response as OkObjectResult;
            
            Assert.IsNotNull(contentResult);
            var result = contentResult.Value as List<PersonDto>;
            Assert.IsNotNull(result);
            Assert.That(result, Is.EquivalentTo(TestDataHelper.GetPersonsResult()));
        }
        
        [Test]
        public async Task WildCard_WhenCalled_ShouldReturnPerson()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService.Setup(x => x.WildCard()).ReturnsAsync(TestDataHelper.GetPerson());
            var mockCompanyService = new Mock<ICompanyService>();
        
            var controller = new PersonController(mockPersonService.Object, mockCompanyService.Object, _mapper);
            var response = await controller.WildCard();
        
            var contentResult = response as OkObjectResult;
            
            Assert.IsNotNull(contentResult);
            var result = contentResult.Value as PersonDto;
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(TestDataHelper.GetPersonResult()));
        }
        
        [Test]
        public async Task AddPerson_WhenIdIsEmptyString_ShouldReturnBadRequest()
        {
            var mockPersonService = new Mock<IPersonService>();
            var mockCompanyService = new Mock<ICompanyService>();
        
            var controller = new PersonController(mockPersonService.Object, mockCompanyService.Object, _mapper);
            var response = await controller.AddPerson(TestDataHelper.PostFailPersonId());
        
            var contentResult = response as BadRequestObjectResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(400, contentResult.StatusCode);
            Assert.AreEqual("Person ID cannot be an empty string",contentResult.Value.ToString());
        }
        
        [Test]
        public async Task AddPerson_WhenPersonAlreadyExists_ShouldReturnConflict()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService.Setup(x => x.PersonExists(It.IsAny<string>())).ReturnsAsync(true);
            var mockCompanyService = new Mock<ICompanyService>();
            
            var controller = new PersonController(mockPersonService.Object, mockCompanyService.Object, _mapper);
            var response = await controller.AddPerson(TestDataHelper.PostPerson());
        
            var contentResult = response as ConflictObjectResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(409, contentResult.StatusCode);
            Assert.AreEqual("Person with ID: 1 already exists!",contentResult.Value.ToString());
        }
        
        [Test]
        public async Task AddPerson_WhenCompanyDoesNotExist_ShouldReturnBadRequest()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService.Setup(x => x.PersonExists(It.IsAny<string>())).ReturnsAsync(false);
            var mockCompanyService = new Mock<ICompanyService>();
            mockCompanyService.Setup(x => x.CompanyExists(It.IsAny<string>())).ReturnsAsync(false);
            
            var controller = new PersonController(mockPersonService.Object, mockCompanyService.Object, _mapper);
            var response = await controller.AddPerson(TestDataHelper.PostPerson());
        
            var contentResult = response as BadRequestObjectResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(400, contentResult.StatusCode);
            Assert.AreEqual("Invalid company name!",contentResult.Value.ToString());
        }
        
        [Test]
        public async Task AddPerson_WhenPersonDoesNotExist_ShouldReturnOk()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService.Setup(x => x.PersonExists(It.IsAny<string>())).ReturnsAsync(false);
            var mockCompanyService = new Mock<ICompanyService>();
            mockCompanyService.Setup(x => x.CompanyExists(It.IsAny<string>())).ReturnsAsync(true);
        
            var controller = new PersonController(mockPersonService.Object, mockCompanyService.Object, _mapper);
            var response = await controller.AddPerson(TestDataHelper.PostPerson());
        
            var contentResult = response as OkResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(200, contentResult.StatusCode);
        }
        
        [Test]
        public async Task EditPerson_WhenPersonDoesNotExist_ShouldReturnBadRequest()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService.Setup(x => x.PersonExists(It.IsAny<string>())).ReturnsAsync(false);
            var mockCompanyService = new Mock<ICompanyService>();
            
            var controller = new PersonController(mockPersonService.Object, mockCompanyService.Object, _mapper);
            var response = await controller.EditPerson("1", TestDataHelper.PostPerson());
        
            var contentResult = response as BadRequestObjectResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(400, contentResult.StatusCode);
            Assert.AreEqual("Person with ID: 1 does not exist!",contentResult.Value.ToString());
        }
        
        [Test]
        public async Task EditPerson_WhenCompanyDoesNotExist_ShouldReturnBadRequest()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService.Setup(x => x.PersonExists(It.IsAny<string>())).ReturnsAsync(true);
            var mockCompanyService = new Mock<ICompanyService>();
            mockCompanyService.Setup(x => x.CompanyExists(It.IsAny<string>())).ReturnsAsync(false);
            
            var controller = new PersonController(mockPersonService.Object, mockCompanyService.Object, _mapper);
            var response = await controller.EditPerson("1", TestDataHelper.PostPerson());
        
            var contentResult = response as BadRequestObjectResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(400, contentResult.StatusCode);
            Assert.AreEqual("Invalid company name!",contentResult.Value.ToString());
        }

        [Test]
        public async Task EditPerson_WhenPersonExistsAndCompanyNameIsValid_ShouldReturnOk()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService.Setup(x => x.PersonExists(It.IsAny<string>())).ReturnsAsync(true);
            var mockCompanyService = new Mock<ICompanyService>();
            mockCompanyService.Setup(x => x.CompanyExists(It.IsAny<string>())).ReturnsAsync(true);
        
            var controller = new PersonController(mockPersonService.Object, mockCompanyService.Object, _mapper);
            var response = await controller.EditPerson("1", TestDataHelper.PostPerson());
        
            var contentResult = response as OkResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(200, contentResult.StatusCode);
        }
        
        [Test]
        public async Task RemovePerson_WhenIdIsEmptyString_ShouldReturnBadRequest()
        {
            var mockPersonService = new Mock<IPersonService>();
            var mockCompanyService = new Mock<ICompanyService>();
            
            var controller = new PersonController(mockPersonService.Object, mockCompanyService.Object, _mapper);
            var response = await controller.RemovePerson("");
        
            var contentResult = response as BadRequestObjectResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(400, contentResult.StatusCode);
            Assert.AreEqual("Person ID cannot be an empty string",contentResult.Value.ToString());
        }
        
        [Test]
        public async Task RemovePerson_WhenPersonDoesNotExist_ShouldReturnBadRequest()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService.Setup(x => x.PersonExists(It.IsAny<string>())).ReturnsAsync(false);
            var mockCompanyService = new Mock<ICompanyService>();
            
            var controller = new PersonController(mockPersonService.Object, mockCompanyService.Object, _mapper);
            var response = await controller.RemovePerson("1");
        
            var contentResult = response as BadRequestObjectResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(400, contentResult.StatusCode);
            Assert.AreEqual("Person with ID: 1 does not exist!",contentResult.Value.ToString());
        }
        
        [Test]
        public async Task RemovePerson_WhenPersonExists_ShouldReturnOk()
        {
            var mockPersonService = new Mock<IPersonService>();
            mockPersonService.Setup(x => x.PersonExists(It.IsAny<string>())).ReturnsAsync(true);
            var mockCompanyService = new Mock<ICompanyService>();
        
            var controller = new PersonController(mockPersonService.Object, mockCompanyService.Object, _mapper);
            var response = await controller.RemovePerson("1");
        
            var contentResult = response as OkResult;
            
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(200, contentResult.StatusCode);
        }
    }
}
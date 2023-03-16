using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Phonebook.Data.Data.Models;
using Phonebook.Data.Services;
using Phonebook.Web.Models;

namespace Phonebook.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper         = mapper;
        }

        [HttpGet]
        [Route("GetAllCompanies")]
        public async Task<IActionResult> GetAllCompanies()
        {
            var response = await _companyService.GetAllCompanies();

            var result = _mapper.Map<List<CompanyDto>>(response);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetPersonCountByCompany")]
        public async Task<IActionResult> GetPersonCountByCompany(string name)
        {
            if (name == "")
            {
                return BadRequest("Please enter a valid company name");
            }

            var response = await _companyService.GetPersonCountByCompany(name);

            if (response == null)
            {
                return BadRequest($"Company with name: {name} does not exist!");
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany([FromBody] CompanyDto companyDto)
        {
            if (companyDto.CompanyName == "")
            {
                return BadRequest("Company Name cannot be an empty string");
            }

            if (companyDto.RegistrationDate > DateTime.Now)
            {
                return BadRequest("Registration Date cannot be in the future");
            }

            if (await _companyService.CompanyExists(companyDto.CompanyName))
            {
                return Conflict($"Company with name: {companyDto.CompanyName} already exists!");
            }

            await _companyService.AddCompany(_mapper.Map<Company>(companyDto));

            return Ok();
        }
    }
}
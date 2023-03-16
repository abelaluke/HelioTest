using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Phonebook.Data.Data.Models;
using Phonebook.Data.Services;
using Phonebook.Web.Models;

namespace Phonebook.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public PersonController(IPersonService personService, ICompanyService companyService, IMapper mapper)
        {
            _personService = personService;
            _companyService = companyService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllPersons")]
        public async Task<IActionResult> GetAllPersons()
        {
            var response = await _personService.GetAllPersons();

            var result = _mapper.Map<List<PersonDto>>(response);

            return Ok(result);
        }

        [HttpGet]
        [Route("SearchPersons")]
        public async Task<IActionResult> SearchPersons(string searchTerm)
        {
            if (searchTerm == "")
            {
                return BadRequest("Search term cannot be an empty string");
            }

            var response = await _personService.SearchPersons(searchTerm);

            var result = _mapper.Map<List<PersonDto>>(response);

            return Ok(result);
        }

        [HttpGet]
        [Route("WildCard")]
        public async Task<IActionResult> WildCard()
        {
            var response = await _personService.WildCard();

            var result = _mapper.Map<PersonDto>(response);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddPerson([FromBody] PersonDto personDto)
        {
            if (personDto.Id == "")
            {
                return BadRequest("Person ID cannot be an empty string");
            }

            if (await _personService.PersonExists(personDto.Id))
            {
                return Conflict($"Person with ID: {personDto.Id} already exists!");
            }

            if (!await _companyService.CompanyExists(personDto.CompanyName))
            {
                return BadRequest("Invalid company name!");
            }

            await _personService.AddPerson(_mapper.Map<Person>(personDto));

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> EditPerson(string id, [FromBody] PersonDto personDto)
        {
            if (!await _personService.PersonExists(id))
            {
                return BadRequest($"Person with ID: {personDto.Id} does not exist!");
            }

            if (!await _companyService.CompanyExists(personDto.CompanyName))
            {
                return BadRequest("Invalid company name!");
            }

            await _personService.EditPerson(id, _mapper.Map<Person>(personDto));

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemovePerson(string id)
        {
            if (id == "")
            {
                return BadRequest("Person ID cannot be an empty string");
            }

            if (!await _personService.PersonExists(id))
            {
                return BadRequest($"Person with ID: {id} does not exist!");
            }

            await _personService.RemovePerson(id);

            return Ok();
        }
    }
}
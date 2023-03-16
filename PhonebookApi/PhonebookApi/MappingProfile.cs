using AutoMapper;
using Phonebook.Data.Data.Models;
using Phonebook.Web.Models;

namespace Phonebook.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>();
            CreateMap<CompanyDto, Company>();
            CreateMap<Person, PersonDto>();
            CreateMap<PersonDto, Person>();
        }
    }
}
using Phonebook.Data.Data.Models;

namespace Phonebook.Data.Services
{
    public interface IPersonService
    {
        Task<bool> PersonExists(string id);
        Task AddPerson(Person person);
        Task<List<Person>> GetAllPersons();
        Task<List<Person>> SearchPersons(string search);
        Task EditPerson(string id, Person person);
        Task RemovePerson(string id);
        Task<Person> WildCard();
    }
}

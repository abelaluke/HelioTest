using Microsoft.EntityFrameworkCore;
using Phonebook.Data.Context;
using Phonebook.Data.Data.Models;

namespace Phonebook.Data.Services
{
    public class PersonService : IPersonService
    {
        private readonly PhonebookContext _dbContext;

        public PersonService(PhonebookContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> PersonExists(string id)
        {
            var person = await _dbContext.Person.FindAsync(id);

            return person != null;
        }

        public async Task AddPerson(Person person)
        {
            await _dbContext.Person.AddAsync(person);
            var company = await _dbContext.Company.FindAsync(person.CompanyName);
            if (company != null)
            {
                company.NumberOfPeople++;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Person>> GetAllPersons()
        {
            return await _dbContext.Person.ToListAsync();
        }

        public async Task<List<Person>> SearchPersons(string searchTerm)
        {
            var persons = await _dbContext.Person
                .Where(p => p.FullName.Contains(searchTerm)
                            || p.Id.Contains(searchTerm)
                            || p.PhoneNumber.Contains(searchTerm)
                            || p.Address.Contains(searchTerm)
                            || p.CompanyName.Contains(searchTerm))
                .ToListAsync();

            return persons;
        }

        public async Task EditPerson(string id, Person person)
        {
            var personToEdit = await _dbContext.Person.FindAsync(id);

            if (personToEdit?.CompanyName != person.CompanyName)
            {
                var reduceCompanyPerson = await _dbContext.Company.FindAsync(personToEdit?.CompanyName);
                if (reduceCompanyPerson != null) reduceCompanyPerson.NumberOfPeople--;
                var addCompanyPerson = await _dbContext.Company.FindAsync(person.CompanyName);
                if (addCompanyPerson != null) addCompanyPerson.NumberOfPeople++;
            }

            if (personToEdit != null)
            {
                personToEdit.FullName = person.FullName;
                personToEdit.PhoneNumber = person.PhoneNumber;
                personToEdit.Address = person.Address;
                personToEdit.CompanyName = person.CompanyName;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task RemovePerson(string id)
        {
            var person = await _dbContext.Person.FindAsync(id);
            if (person != null)
            {
                _dbContext.Person.Remove(person);
                var company = await _dbContext.Company.FindAsync(person.CompanyName);
                if (company != null)
                {
                    company.NumberOfPeople--;
                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Person> WildCard()
        {
            var count = await _dbContext.Person.CountAsync();
            var randomIndex = new Random().Next(count);
            return await _dbContext.Person
                .Skip(randomIndex)
                .FirstOrDefaultAsync();
        }
    }
}
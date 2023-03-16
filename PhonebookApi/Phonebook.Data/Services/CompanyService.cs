using Microsoft.EntityFrameworkCore;
using Phonebook.Data.Context;
using Phonebook.Data.Data.Models;

namespace Phonebook.Data.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly PhonebookContext _dbContext;

        public CompanyService(PhonebookContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CompanyExists(string name)
        {
            var company = await _dbContext.Company.FindAsync(name);

            return company != null;
        }

        public async Task<List<Company>> GetAllCompanies()
        {
            return await _dbContext.Company.ToListAsync();
        }

        public async Task<int?> GetPersonCountByCompany(string name)
        {
            var company = await _dbContext.Company.FindAsync(name);

            return company?.NumberOfPeople;
        }

        public async Task AddCompany(Company company)
        {
            await _dbContext.Company.AddAsync(company);
            await _dbContext.SaveChangesAsync();
        }
    }
}
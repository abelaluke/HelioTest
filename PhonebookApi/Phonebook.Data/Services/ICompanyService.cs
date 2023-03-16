using Phonebook.Data.Data.Models;

namespace Phonebook.Data.Services
{
    public interface ICompanyService
    {
        Task<bool> CompanyExists(string name);
        Task<List<Company>> GetAllCompanies();
        Task<int?> GetPersonCountByCompany(string name);
        Task AddCompany(Company company);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phonebook.Data.Context;

namespace Phonebook.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPhonebookConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PhonebookContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("PhonebookDb")));

            return services;
        }
    }
}
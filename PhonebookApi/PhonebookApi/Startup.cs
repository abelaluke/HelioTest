using AutoMapper;
using Phonebook.Data;
using Phonebook.Data.Context;
using Phonebook.Data.Services;

namespace Phonebook.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddPhonebookConfig(configuration);
            services.AddDbContext<PhonebookContext>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
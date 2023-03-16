using Microsoft.EntityFrameworkCore;
using Phonebook.Data.Data.Models;

namespace Phonebook.Data.Context
{
    public class PhonebookContext : DbContext
    {
        public DbSet<Company> Company { get; set; }
        public DbSet<Person> Person { get; set; }

        public PhonebookContext(DbContextOptions<PhonebookContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasKey(c => c.CompanyName);
            modelBuilder.Entity<Person>().HasKey(p => p.Id);
        }
    }
}
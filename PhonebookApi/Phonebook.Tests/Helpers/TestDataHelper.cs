using System;
using System.Collections.Generic;
using Phonebook.Data.Data.Models;
using Phonebook.Web.Models;

namespace Phonebook.Tests.Helpers
{
    public abstract class TestDataHelper
    {
        private static readonly DateTime SetDate = new(2023, 1, 1, 0, 0, 0);

        public static CompanyDto PostCompany()
        {
            return new CompanyDto
            {
                CompanyName      = "TestCompany",
                RegistrationDate = SetDate
            };
        }

        public static CompanyDto PostFailCompanyName()
        {
            return new CompanyDto
            {
                CompanyName      = "",
                RegistrationDate = SetDate
            };
        }

        public static CompanyDto PostFailRegistrationDate()
        {
            return new CompanyDto
            {
                CompanyName      = "Test",
                RegistrationDate = new DateTime(2024, 1, 1, 0, 0, 0)
            };
        }

        public static List<Company> GetCompanies()
        {
            return new List<Company>
            {
                new()
                {
                    CompanyName      = "TestCompany",
                    RegistrationDate = SetDate,
                    NumberOfPeople   = 0
                },
                new()
                {
                    CompanyName      = "TestCompany1",
                    RegistrationDate = SetDate,
                    NumberOfPeople   = 1
                },
                new()
                {
                    CompanyName      = "TestCompany2",
                    RegistrationDate = SetDate,
                    NumberOfPeople   = 2
                },
                new()
                {
                    CompanyName      = "TestCompany3",
                    RegistrationDate = SetDate,
                    NumberOfPeople   = 3
                }
            };
        }

        public static List<CompanyDto> GetCompaniesResult()
        {
            return new List<CompanyDto>
            {
                new()
                {
                    CompanyName      = "TestCompany",
                    RegistrationDate = SetDate
                },
                new()
                {
                    CompanyName      = "TestCompany1",
                    RegistrationDate = SetDate
                },
                new()
                {
                    CompanyName      = "TestCompany2",
                    RegistrationDate = SetDate
                },
                new()
                {
                    CompanyName      = "TestCompany3",
                    RegistrationDate = SetDate,
                }
            };
        }

        public static PersonDto PostPerson()
        {
            return new PersonDto
            {
                Id          = "1",
                FullName    = "Test",
                PhoneNumber = "+356 99887766",
                Address     = "1, Test Street, Test Locality, Malta",
                CompanyName = "TestCompany"
            };
        }

        public static PersonDto PostFailPersonId()
        {
            return new PersonDto
            {
                Id          = "",
                FullName    = "Test",
                PhoneNumber = "+356 99887766",
                Address     = "1, Test Street, Test Locality, Malta",
                CompanyName = "TestCompany"
            };
        }

        public static List<Person> GetPersons()
        {
            return new List<Person>
            {
                new()
                {
                    Id          = "1",
                    FullName    = "Test1",
                    PhoneNumber = "+356 99887766",
                    Address     = "1, Test Street, Test Locality, Malta",
                    CompanyName = "TestCompany"
                },
                new()
                {
                    Id          = "2",
                    FullName    = "Test2",
                    PhoneNumber = "+356 99887766",
                    Address     = "1, Test Street, Test Locality, Malta",
                    CompanyName = "TestCompany1"
                },
                new()
                {
                    Id          = "3",
                    FullName    = "Test3",
                    PhoneNumber = "+356 99887766",
                    Address     = "1, Test Street, Test Locality, Malta",
                    CompanyName = "TestCompany2"
                }
            };
        }

        public static Person GetPerson()
        {
            return new Person
            {
                Id          = "1",
                FullName    = "Test1",
                PhoneNumber = "+356 99887766",
                Address     = "1, Test Street, Test Locality, Malta",
                CompanyName = "TestCompany"
            };
        }

        public static PersonDto GetPersonResult()
        {
            return new PersonDto
            {
                Id          = "1",
                FullName    = "Test1",
                PhoneNumber = "+356 99887766",
                Address     = "1, Test Street, Test Locality, Malta",
                CompanyName = "TestCompany"
            };
        }

        public static List<PersonDto> GetPersonsResult()
        {
            return new List<PersonDto>
            {
                new()
                {
                    Id          = "1",
                    FullName    = "Test1",
                    PhoneNumber = "+356 99887766",
                    Address     = "1, Test Street, Test Locality, Malta",
                    CompanyName = "TestCompany"
                },
                new()
                {
                    Id          = "2",
                    FullName    = "Test2",
                    PhoneNumber = "+356 99887766",
                    Address     = "1, Test Street, Test Locality, Malta",
                    CompanyName = "TestCompany1"
                },
                new()
                {
                    Id          = "3",
                    FullName    = "Test3",
                    PhoneNumber = "+356 99887766",
                    Address     = "1, Test Street, Test Locality, Malta",
                    CompanyName = "TestCompany2"
                }
            };
        }
    }
}
namespace Phonebook.Web.Models
{
    public class CompanyDto
    {
        public string CompanyName { get; set; }
        public DateTime RegistrationDate { get; set; }

        public override bool Equals(object? obj)
        {
            return CompanyName == (obj as CompanyDto)?.CompanyName && RegistrationDate == (obj as CompanyDto)?.RegistrationDate;
        }

        protected bool Equals(CompanyDto other)
        {
            return CompanyName == other.CompanyName && RegistrationDate.Equals(other.RegistrationDate);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CompanyName, RegistrationDate);
        }
    }
}
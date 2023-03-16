namespace Phonebook.Web.Models
{
    public class PersonDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        
        public override bool Equals(object? obj)
        {
            return Id == (obj as PersonDto)?.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FullName, PhoneNumber, Address, CompanyName);
        }
    }
}
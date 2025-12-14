namespace SIS
{
    public class ContactPerson : IPerson
    {
        public int ContactPersonId { get; set; }

        // FK
        public int InternshipId { get; set; }

        public Internship Internship { get; set; } = null!;

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public string FunctionTitle { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;

        public string GetFullName() => $"{FirstName} {LastName}";
    }
}

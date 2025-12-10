namespace SIS;

public class ContactPerson : IPerson
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public string? FunctionTitle { get; set; }
    public string? DepartmentName { get; set; }


    public int InternshipId { get; set; }
    public Internship? Internship { get; set; }

    public string GetFullName() => $"{FirstName} {LastName}";
}

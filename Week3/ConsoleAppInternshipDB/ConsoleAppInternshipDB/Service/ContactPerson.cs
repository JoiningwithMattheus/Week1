namespace SIS;

public class ContactPerson : IPerson
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public string FunctionTitle { get; set; }
    public string DepartmentName { get; set; }

    public ContactPerson(string firstName, string lastName, string email, string phone, string function, string department)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phone;
        FunctionTitle = function;
        DepartmentName = department;
    }

    public string GetFullName() => $"{FirstName} {LastName}";
}

namespace SIS;

public interface IPerson
{
    string FirstName { get; set; }
    string LastName { get; set; }
    string Email { get; set; }
    string PhoneNumber { get; set; }

    string GetFullName();
}

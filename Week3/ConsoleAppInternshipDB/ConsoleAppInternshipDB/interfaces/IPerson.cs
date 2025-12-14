namespace SIS;

public interface IPerson
{
    string FirstName { get; }
    string LastName { get; }
    string Email { get; }
    string PhoneNumber { get; }

    string GetFullName();
}

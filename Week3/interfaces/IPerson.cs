namespace SIS
{
    interface IPerson
    {
        string FirstName { get; }
        string LastName { get; }
        string PhoneNumber { get; }
        string Email { get; }
        string GetFullName();
    }
}
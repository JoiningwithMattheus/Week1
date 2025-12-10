namespace SIS
{
    public class Student : IPerson, IUser
    {
        public int StudentId { get; set; }
        public uint StudentNumber { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public List<Application> Applications { get; private set; } = new();

        public Student() { }

        public Student(uint studentNumber, string firstName, string lastName, string email, string phone)
        {
            StudentNumber = studentNumber;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phone;
        }

        public string GetFullName() => $"{FirstName} {LastName}";

        public bool Login() => true;

        public void ShowMenu()
        {
            Console.WriteLine("Student Menu:");
        }
    }
}

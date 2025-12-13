using Microsoft.EntityFrameworkCore;

namespace SIS
{
    public class Student : IPerson, IUser
    {
        public int StudentId { get; set; }
        public uint StudentNumber { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public List<Application> Applications { get; private set; } = new();

        private readonly SisDbContext _db;

        public Student() { }

        public Student(uint studentNumber, string firstName, string lastName, string email, string phone)
        {
            StudentNumber = studentNumber;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phone;

            _db = new SisDbContext();
        }

        public string GetFullName() => $"{FirstName} {LastName}";

        public bool Login() => true;

        public void ShowMenu()
        {
            Console.WriteLine("\n===== STUDENT MENU =====");
            Console.WriteLine("1. Show Available Internships");
            Console.WriteLine("2. View Single Internship");
            Console.WriteLine("3. Apply for Internship");
        }

        public void GetAvailableInternship(Period period, InternshipCategory category)
        {
            List<Internship> internships = ListInternshipsAsync(period, category).GetAwaiter().GetResult();

            if (internships == null)
            {
                Console.WriteLine("No internships available for the selected period and category.");
                return;
            }
            
            Console.WriteLine($"Available Internships ({category}):");
            
            for (int i = 0; i < internships.Count; i++)
            {
                var internship = internships[i];
                Console.WriteLine($"{i + 1}.");
                Console.WriteLine($"   Organization: {internship.Organization.Name}");
                Console.WriteLine($"   Address: {internship.Organization.Address}");
                Console.WriteLine($"   Date of Submission: {internship.DateOfSubmission}");
                Console.WriteLine($"   Project Title: {internship.ProjectTitle}");
                Console.WriteLine($"   Short Description: {internship.ShortDescription}");
                Console.WriteLine();
            }
            
        }

        public async Task<List<Internship>> ListInternshipsAsync(Period period, InternshipCategory category)
        {
            return await _db.Internships
                .Where(i =>
                     i.Period.Year == period.Year &&
                     i.Period.Semester == period.Semester &&
                    (
                        (category == InternshipCategory.INTERMEDIATE && i is IntermediateInternship) ||
                        (category == InternshipCategory.MINOR && i is MinorInternship) ||
                        (category == InternshipCategory.GRADUATION && i is GraduationInternship)
                    )
                )
                .ToListAsync();
        }

        
    }
}

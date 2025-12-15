using System.Collections.Specialized;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SIS
{
    public class Student : IPerson, IUser
    {
        public int StudentId { get; set; }
        public uint StudentNumber { get; set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }

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

        private static async Task<Student?> LoginStudent()
        {
            Console.WriteLine("Enter student number:");
            uint studentNumber = uint.Parse(Console.ReadLine()!);
            
            var student = await db.Students.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
            if (student != null) return student;

            await db.SaveChangesAsync();
            return student;
        }
        public bool Login()
        {
            using var db = new SisDbContext();
            Console.WriteLine("Enter Student Number:");
            string? input = Console.ReadLine();
            uint loginnum;
            if (uint.TryParse(input, out loginnum))
            {
                foreach (Student student in _db.Students)
                {
                    if (student.StudentNumber == loginnum)
                    {
                        StudentNumber = loginnum;
                        FirstName = student.FirstName;
                        LastName = student.LastName;
                        Email = student.Email;
                        PhoneNumber = student.PhoneNumber;
                        Applications = student.Applications;
                        return true;
                    }
                }
            }
            Console.WriteLine("Invalid Login.");
            return false;
        }

        public void ShowMenu()
        {
            bool quit = false;
            while (!quit){
                Console.WriteLine("\nSTUDENT MENU:");
                Console.WriteLine("1. Show Available Internships");
                Console.WriteLine("2. Apply for Internship");
                Console.WriteLine("3. View current Applications");
                Console.WriteLine("Q. quit");
                string? input = Console.ReadLine();
                if (input == "1")
                {
                    // Get period from user
                    Console.Write("Enter year: ");
                    int year = int.Parse(Console.ReadLine() ?? "2025");
                    // defaults to 2025 if nothing entered
                    
                    Console.WriteLine("Select semester:");
                    Console.WriteLine("1. Semester 1");
                    Console.WriteLine("2. Semester 2");
                    string? semesterInput = Console.ReadLine();

                    Semester semester = semesterInput == "1" ? Semester.One : Semester.Two; //temp defaults to Two if anything else

                    // Get category from user
                    Console.WriteLine("Select internship category:");
                    Console.WriteLine("1. MINOR");
                    Console.WriteLine("2. INTERMEDIATE");
                    Console.WriteLine("3. GRADUATION");
                    string? categoryInput = Console.ReadLine();

                    InternshipCategory category = categoryInput switch
                    {
                        "1" => InternshipCategory.MINOR,
                        "2" => InternshipCategory.INTERMEDIATE,
                        "3" => InternshipCategory.GRADUATION,
                        _ => InternshipCategory.MINOR //temp defaults to minor
                    };

                    Period period = new Period(year, semester);
                    GetAvailableInternship(period, category);
                }
                else if (input == "2")
                {
                    //TODO: Implement application function
                    Console.WriteLine("Application Process not yet implemented");
                }
                else if (input == "3")
                {
                    ShowApplications();
                }
                else if (input == "Q" || input == "q" || input == "Quit" || input == "quit")
                {
                    Console.WriteLine("Quitting.");
                    quit = true;
                }
                else
                {
                    Console.WriteLine("Invalid Option, try again.");
                }
            }
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

            while (true)
            {
                Console.WriteLine("Enter a number to view the full description of an internship, or M to return to Menu.");
                string? input = Console.ReadLine();
                int num;
                if (int.TryParse(input, out num))
                {
                    var internship = internships[num];
                    Console.WriteLine($"Organization: {internship.Organization.Name}");
                    Console.WriteLine($"Address: {internship.Organization.Address}");
                    Console.WriteLine($"Date of Submission: {internship.DateOfSubmission}");
                    Console.WriteLine($"Project Title: {internship.ProjectTitle}");
                    Console.WriteLine($"Long Description: {internship.LongDescription}");
                    Console.WriteLine($"Contact Persons:");
                    foreach (ContactPerson contact in internship.ContactPersons)
                    {
                        Console.WriteLine($"   Name: {contact.FirstName} {contact.LastName}");
                        Console.WriteLine($"   Email: {contact.Email}");
                        Console.WriteLine($"   Phone Number: {contact.PhoneNumber}");
                        Console.WriteLine($"   Function Title: {contact.FunctionTitle}");
                        Console.WriteLine($"   Department Name: {contact.DepartmentName}");
                    }
                }
                else if (input == "M" || input == "m")
                {
                    return;
                }
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

        public void ShowApplications()
        {
            if (Applications.Count > 0) 
            {
                foreach (Application application in Applications)
                {
                    Console.WriteLine(application.Summary());
                }
            }
            else
            {
                Console.WriteLine("No current applications.");
            }
        }


        
    }
}
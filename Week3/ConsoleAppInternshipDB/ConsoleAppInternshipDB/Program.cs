using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SIS
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Welcome to SIS System");
            while (true)
            {
                Console.WriteLine("\nSelect user type:");
                Console.WriteLine("1. Student");
                Console.WriteLine("2. Coordinator");
                Console.WriteLine("0. Exit");

                var choice = Console.ReadLine();
                if (choice == "0") break;

                switch (choice)
                {
                    case "1":
                        await HandleStudentMenuAsync();
                        break;
                    case "2":
                        await HandleCoordinatorMenuAsync();
                        break;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
            }
        }

        private static async Task HandleStudentMenuAsync()
        {
            var student = await CreateOrLoginStudentAsync();
            if (student == null) return;

            while (true)
            {
                student.ShowMenu();
                var input = Console.ReadLine();
                if (input == "0") break;

                switch (input)
                {
                    case "1":
                        await ShowAvailableInternships(student);
                        break;
                    case "2":
                        await ApplyForInternship(student);
                        break;
                    case "3":
                        ShowApplications(student);
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private static async Task HandleCoordinatorMenuAsync()
        {
            var coordinator = await CreateOrLoginCoordinatorAsync();
            if (coordinator == null) return;

            while (true)
            {
                coordinator.ShowMenu();
                var input = Console.ReadLine();
                if (input == "0") break;

                switch (input)
                {
                    case "1":
                        await AddOrganisation(coordinator);
                        break;
                    case "2":
                        await RemoveOrganisation(coordinator);
                        break;
                    case "3":
                        await ListOrganisations(coordinator);
                        break;
                    case "4":
                        await AddInternship(coordinator);
                        break;
                    case "5":
                        await WithdrawInternships(coordinator);
                        break;
                    case "7": // Get Internship Contacts
                        await ShowContacts(coordinator);
                        break;
                    case "10": // Mark internship as complete
                        await MarkInternshipComplete(coordinator);
                        break;
                    default:
                        Console.WriteLine("Feature not implemented yet.");
                        break;
                }
            }
        }

        private static async Task<Student?> CreateOrLoginStudentAsync()
        {
            Console.WriteLine("Enter student number:");
            uint studentNumber = uint.Parse(Console.ReadLine()!);
            using var db = new SisDbContext();
            var student = await db.Students.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
            if (student != null) return student;

            Console.WriteLine("Student not found. Creating new student.");
            Console.WriteLine("Enter first name:");
            string firstName = Console.ReadLine()!;
            Console.WriteLine("Enter last name:");
            string lastName = Console.ReadLine()!;
            Console.WriteLine("Enter email:");
            string email = Console.ReadLine()!;
            Console.WriteLine("Enter phone number:");
            string phone = Console.ReadLine()!;

            student = new Student(studentNumber, firstName, lastName, email, phone);
            db.Students.Add(student);
            await db.SaveChangesAsync();
            return student;
        }

        private static async Task ShowAvailableInternships(Student student)
        {
            Console.WriteLine("Enter period year:");
            int year = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter semester (1 or 2):");
            Semester semester = Console.ReadLine()! == "1" ? Semester.One : Semester.Two;
            var period = new Period(year, semester);

            Console.WriteLine("Enter internship category (1=Intermediate,2=Minor,3=Graduation):");
            InternshipCategory category = Console.ReadLine()! switch
            {
                "1" => InternshipCategory.INTERMEDIATE,
                "2" => InternshipCategory.MINOR,
                "3" => InternshipCategory.GRADUATION,
                _ => InternshipCategory.INTERMEDIATE
            };

            var internships = await student.GetAvailableInternshipAsync(period, category);
            Console.WriteLine("Available Internships:");
            foreach (var i in internships)
                Console.WriteLine($"{i.Id}: {i.ProjectTitle} ({i.Status})");
        }

        private static async Task ApplyForInternship(Student student)
        {
            Console.WriteLine("Enter Internship Id to apply:");
            int internshipId = int.Parse(Console.ReadLine()!);
            using var db = new SisDbContext();
            var internship = await db.Internships.FindAsync(internshipId);
            if (internship == null)
            {
                Console.WriteLine("Internship not found.");
                return;
            }

            var app = await student.ApplyForInternshipAsync(internship);
            Console.WriteLine($"Application submitted: {app.ApplicationID}, Status: {app.Status}");
        }

        private static void ShowApplications(Student student)
        {
            var apps = student.GetApplications();
            if (apps.Count == 0) Console.WriteLine("No applications.");
            foreach (var a in apps)
                Console.WriteLine($"{a.ApplicationID}: {a.Internship.ProjectTitle}, Status: {a.Status}");
        }

        private static async Task<Coordinator?> CreateOrLoginCoordinatorAsync()
        {
            Console.WriteLine("Enter coordinator first name:");
            string firstName = Console.ReadLine()!;
            Console.WriteLine("Enter last name:");
            string lastName = Console.ReadLine()!;
            Console.WriteLine("Enter email:");
            string email = Console.ReadLine()!;
            Console.WriteLine("Enter phone:");
            string phone = Console.ReadLine()!;
            return new Coordinator(firstName, lastName, email, phone);
        }

        private static async Task AddOrganisation(Coordinator coordinator)
        {
            Console.WriteLine("Enter company name:");
            string name = Console.ReadLine()!;
            Console.WriteLine("Enter address:");
            string address = Console.ReadLine()!;
            Console.WriteLine("Enter email:");
            string email = Console.ReadLine()!;
            Console.WriteLine("Enter phone:");
            string phone = Console.ReadLine()!;
            Console.WriteLine("Enter URL:");
            string url = Console.ReadLine()!;

            var org = new Company
            {
                Name = name,
                Address = address,
                Email = email,
                PhoneNumber = phone,
                Url = url
            };

            await coordinator.AddOrganisationAsync(org);
            Console.WriteLine("Organisation added successfully.");
        }

        private static async Task AddInternship(Coordinator coordinator)
        {
            Console.WriteLine("Enter internship title:");
            string title = Console.ReadLine()!;
            Console.WriteLine("Enter short description:");
            string shortDesc = Console.ReadLine()!;
            Console.WriteLine("Enter long description:");
            string longDesc = Console.ReadLine()!;
            Console.WriteLine("Enter capacity:");
            int capacity = int.Parse(Console.ReadLine()!);

            Console.WriteLine("Enter period year:");
            int year = int.Parse(Console.ReadLine()!);
            Console.WriteLine("Enter semester (1 or 2):");
            Semester semester = Console.ReadLine()! == "1" ? Semester.One : Semester.Two;

            Console.WriteLine("Enter company id:");
            int companyId = int.Parse(Console.ReadLine()!);

            var internship = new IntermediateInternship
            {
                ProjectTitle = title,
                ShortDescription = shortDesc,
                LongDescription = longDesc,
                Capacity = capacity,
                DateOfSubmission = DateTime.Now,
                Period = new Period(year, semester)
            };

            await coordinator.AddInternshipAsync(internship, companyId);
            Console.WriteLine("Internship added successfully.");
        }

        private static async Task ShowContacts(Coordinator coordinator)
        {
            Console.WriteLine("Enter Internship id:");
            int internshipId = int.Parse(Console.ReadLine()!);
            using var db = new SisDbContext();
            var internship = await db.Internships.FindAsync(internshipId);

            if (internship != null)
            {
                foreach (var contact in internship.ContactPersons)
                {
                    Console.WriteLine($"Name: {contact.GetFullName()}");
                    Console.WriteLine($"    Email: {contact.Email}");
                    Console.WriteLine($"    Phone number: {contact.PhoneNumber}");
                    Console.WriteLine($"    Function: {contact.FunctionTitle}");
                    Console.WriteLine($"    Department: {contact.DepartmentName}");
                }            
            }
            else
            {
                Console.WriteLine("Invalid ID");
            }
        }

        private static async Task MarkInternshipComplete(Coordinator coordinator)
        {
            Console.WriteLine("Enter Internship id:");
            int internshipId = int.Parse(Console.ReadLine()!);
            using var db = new SisDbContext();
            var internship = await db.Internships.FindAsync(internshipId);

            if (internship != null)
            {
                internship.Status = InternshipStatus.COMPLETED;
                Console.WriteLine("Marked as complete.");         
            }
            else
            {
                Console.WriteLine("Invalid ID");
            }
        }


        private static async Task RemoveOrganisation(Coordinator coordinator)
        {
            Console.WriteLine("Enter the organisation ID: ");
            int orgId = Convert.ToInt32(Console.ReadLine()!);

            Console.WriteLine("Enter Y to confirm!");
            string conf = Console.ReadLine()!;
            if (conf == "Y" || conf == "y")
            {
                await coordinator.RemoveOrganisationAsync(orgId);
                Console.WriteLine("Organisation removed successfully.");
            }
            else
            {
                Console.WriteLine("Organisation has not been removed yet.");
            }
        }
        private static async Task ListOrganisations(Coordinator coordinator)
        {
            var organizations = await coordinator.ListOrganisationsAsync();
            Console.WriteLine("Available Organisations:");
            foreach (var i in organizations)
                Console.WriteLine($"{i.Id}: {i.Name} \n ({i.PhoneNumber} - {i.Address})");
        }
        private static async Task WithdrawInternships(Coordinator coordinator)
        {
            Console.WriteLine("Enter the Internship ID: ");
            int intID = Convert.ToInt32(Console.ReadLine()!);

            Console.WriteLine("Enter Y to confirm!");
            string conf = Console.ReadLine()!;
            if (conf == "Y" || conf == "y")
            {
                await coordinator.WithdrawInternshipAsync(intID);
                Console.WriteLine("Internship removed successfully.");
            }
            else
            {
                Console.WriteLine("Internship has not been withdrawn yet.");
            }
        }
    }
}

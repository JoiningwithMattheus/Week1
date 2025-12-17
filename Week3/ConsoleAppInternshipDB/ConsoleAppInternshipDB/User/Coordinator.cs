using Microsoft.EntityFrameworkCore;

namespace SIS
{
    public class Coordinator : IPerson, IUser
    {
        public int CoordinatorId { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public string GetFullName() => $"{FirstName} {LastName}";

        public Coordinator(string firstName, string lastName, string email, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public bool Login()
        {
            return true;
        }

        public void ShowMenu()
        {
            Console.WriteLine("\nCOORDINATOR MENU:\n");
            Console.WriteLine("1. Add Organisation");
            Console.WriteLine("2. Remove Organisation");
            Console.WriteLine("3. List Organisations");
            Console.WriteLine("4. Add Internship");
            Console.WriteLine("5. Withdraw Internship");
            Console.WriteLine("6. List Internships");
            Console.WriteLine("7. Get Internship Contact Persons");
            Console.WriteLine("8. Process Application");
            Console.WriteLine("9. Assign Student To Internship");
            Console.WriteLine("10. Mark Internship Completed");
        }

        public async Task<bool> AddOrganisationAsync(Organization org)
        {
            using var db = new SisDbContext();
            db.Organizations.Add(org);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveOrganisationAsync(int orgId)
        {
            using var db = new SisDbContext();
            var org = await db.Organizations.FindAsync(orgId);
            if (org == null) return false;

            db.Organizations.Remove(org);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Organization>> ListOrganisationsAsync()
        {
            using var db = new SisDbContext();
            return await db.Organizations.ToListAsync();
        }

        public async Task AddInternshipAsync(Internship internship, int orgId)
        {
            using var db = new SisDbContext();
            var org = await db.Organizations.FindAsync(orgId);
            if (org == null) throw new Exception("Organization not found.");

            internship.Organization = org;

            db.Internships.Add(internship);
            await db.SaveChangesAsync();
        }

        public async Task WithdrawInternshipAsync(int internshipId)
        {
            using var db = new SisDbContext();
            var internship = await db.Internships.FindAsync(internshipId);
            if (internship == null) return;

            internship.Status = InternshipStatus.WITHDRAWN;
            await db.SaveChangesAsync();
        }

        public async Task<List<Internship>> ListInternshipsAsync(Period period, InternshipCategory category)
        {
            using var db = new SisDbContext();
            return await db.Internships
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

        public async Task<List<ContactPerson>> GetContactPersonsAsync(int internshipId)
        {
            using var db = new SisDbContext();
            return await db.ContactPersons
                .Where(cp => cp.InternshipId == internshipId)
                .ToListAsync();
        }

        public bool ProcessAssignment(Application application)
        {
            application.Status = ApplicationStatus.APPROVED;
            return true;
        }

        public async Task AssignStudentToInternshipAsync(uint studentNumber, int internshipId)
        {
            using var db = new SisDbContext();
            var managedStudent = await db.Students.FirstOrDefaultAsync(s => s.StudentNumber == studentNumber)
                ?? throw new InvalidOperationException("Student not found");

            var managedInternship = await db.Internships
                .Include(i => i.AssignedStudents)
                .FirstOrDefaultAsync(i => i.Id == internshipId)
                ?? throw new InvalidOperationException("Internship not found");

            managedInternship.AssignStudents(managedStudent);
            await db.SaveChangesAsync();
        }

        public async Task MarkInternshipCompletedAsync(Internship internship, float finalGrade)
        {
            using var db = new SisDbContext();
            internship.FinalGrade = finalGrade;
            internship.Status = InternshipStatus.COMPLETED;
            await db.SaveChangesAsync();
        }
    }
}
using Microsoft.EntityFrameworkCore;

namespace SIS
{
    public class Coordinator : IPerson, IUser
    {
        public int CoordinatorId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string GetFullName() => $"{FirstName} {LastName}";

        private readonly SisDbContext _db;

        public Coordinator(string firstName, string lastName, string email, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;

            _db = new SisDbContext();
        }

        public bool Login()
        {
            // In real systems you'd check password, etc.
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
            _db.Organizations.Add(org);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveOrganisationAsync(int orgId)
        {
            var org = await _db.Organizations.FindAsync(orgId);
            if (org == null) return false;

            _db.Organizations.Remove(org);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Organization>> ListOrganisationsAsync()
        {
            return await _db.Organizations.ToListAsync();
        }

        public async Task AddInternshipAsync(Internship internship, int orgId)
        {
            var org = await _db.Organizations.FindAsync(orgId);
            if (org == null) throw new Exception("Organization not found.");

            internship.Organization = org;

            _db.Internships.Add(internship);
            await _db.SaveChangesAsync();
        }

        public async Task WithdrawInternshipAsync(int internshipId)
        {
            var internship = await _db.Internships.FindAsync(internshipId);
            if (internship == null) return;

            internship.Status = InternshipStatus.WITHDRAWN;
            await _db.SaveChangesAsync();
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

        public async Task<List<ContactPerson>> GetContactPersonsAsync(int internshipId)
        {
            return await _db.ContactPersons
                .Where(cp => cp.InternshipId == internshipId)
                .ToListAsync();
        }

        public bool ProcessAssignment(Application application)
        {
            application.Status = ApplicationStatus.APPROVED;
            return true;
        }

        public async Task AssignStudentToInternshipAsync(Student student, Internship internship)
        {
            internship.AssignStudents(student);
            await _db.SaveChangesAsync();
        }


        public async Task MarkInternshipCompletedAsync(Internship internship, float finalGrade)
        {
            internship.FinalGrade = finalGrade;
            internship.Status = InternshipStatus.COMPLETED;
            await _db.SaveChangesAsync();
        }
    }
}
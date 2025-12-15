using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SIS
{
    public class Student : IPerson, IUser
    {
        public int StudentId { get; set; }
        public uint StudentNumber { get; set; }

        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;

        // backing list + read-only view
        private readonly List<Application> _applications = new();
        public IReadOnlyCollection<Application> Applications => _applications.AsReadOnly();

        public Assignment? Assignment { get; private set; }

        private readonly SisDbContext _db;

        // EF Core requires a parameterless ctor; keep it private
        private Student()
        {
            // keep string props non-null for nullable analysis
            FirstName = LastName = Email = PhoneNumber = string.Empty;
            _db = null!; // EF will set values when materializing
        }

        public Student(
            uint studentNumber,
            string firstName,
            string lastName,
            string email,
            string phoneNumber)
        {
            StudentNumber = studentNumber;
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));

            _db = new SisDbContext();
        }

        public string GetFullName() => $"{FirstName} {LastName}";

        public bool Login()
        {
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
            Console.WriteLine("Student Menu:");
            Console.WriteLine("1. View Available Internships");
            Console.WriteLine("2. Apply for Internship");
            Console.WriteLine("3. View Applications");
        }

        public async Task<List<Internship>> GetAvailableInternshipAsync(Period period, InternshipCategory category)
        {
            return await _db.Internships
                .Where(i =>
                    i.Status == InternshipStatus.OPEN &&
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

        // SAFER Apply: ensure we work with entities tracked by this context
        public async Task<Application> ApplyForInternshipAsync(Internship internship)
        {
            if (internship == null) throw new ArgumentNullException(nameof(internship));

            // Ensure the student exists in this context (by unique StudentNumber)
            var managedStudent = await _db.Students
                .FirstOrDefaultAsync(s => s.StudentNumber == this.StudentNumber);

            if (managedStudent == null)
            {
                // If the student isn't yet persisted in this DB instance, persist it here
                // and then re-fetch to have a managed instance.
                _db.Students.Add(this);
                await _db.SaveChangesAsync();

                managedStudent = await _db.Students
                    .FirstOrDefaultAsync(s => s.StudentNumber == this.StudentNumber);

                if (managedStudent == null)
                    throw new InvalidOperationException("Failed to create or retrieve the student.");
            }

            // Load the internship from this same context (include Organization to avoid re-insert)
            var managedInternship = await _db.Internships
                .Include(i => i.Organization)
                .FirstOrDefaultAsync(i => i.Id == internship.Id);

            if (managedInternship == null)
                throw new InvalidOperationException("Internship not found in the database. Make sure the internship was created and saved first.");

            // Create application linking managed entities
            var application = new Application
            {
                Student = managedStudent,
                Internship = managedInternship,
                Status = ApplicationStatus.SUBMITTED,
                SubmittedAt = DateTime.UtcNow,
                Motivation = string.Empty
            };

            _db.Applications.Add(application);
            await _db.SaveChangesAsync();

            // update local backing list (so in-memory state matches DB)
            _applications.Add(application);

            return application;
        }

        public IReadOnlyCollection<Application> GetApplications()
        {
            return Applications;
        }

        public bool HasApplied(Period period)
        {
            if (period == null) throw new ArgumentNullException(nameof(period));

            return _applications.Any(a =>
                a.Internship != null &&
                a.Internship.Period != null &&
                a.Internship.Period.Year == period.Year &&
                a.Internship.Period.Semester == period.Semester);
        }
    }
}


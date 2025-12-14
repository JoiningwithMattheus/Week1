namespace SIS
{
    public class Period
    {
        public Period() { }

        public Period(int year, Semester semester)
        {
            Year = year;
            Semester = semester;
        }

        public int Year { get; set; }
        public Semester Semester { get; set; }
    }

    public class Internship
    {
        public int Id { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public string? ProjectTitle { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public float? FinalGrade { get; set; }
        public InternshipStatus Status { get; set; } = InternshipStatus.OPEN;
        public int Capacity { get; set; } = 1;

        private readonly List<Student> _assignedStudents = new();
        public IReadOnlyCollection<Student> AssignedStudents => _assignedStudents;

        public Period Period { get; set; } = new Period(0, Semester.One);
        public Organization? Organization { get; set; }
        public ICollection<ContactPerson> ContactPersons { get; set; }
            = new List<ContactPerson>();

        public void AddContactPerson(ContactPerson cp)
        {
            if (cp == null)
                throw new ArgumentNullException(nameof(cp));

            ContactPersons.Add(cp);
        }

        public void AssignStudents(Student s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            if (_assignedStudents.Count >= Capacity)
                throw new InvalidOperationException("No slot left!");

            _assignedStudents.Add(s);

            if (_assignedStudents.Count >= Capacity)
                Status = InternshipStatus.ASSIGNED;
        }

        public string Overview()
        {
            return $"{ProjectTitle} : {Status} \n {ShortDescription}";
        }
    }

    public class IntermediateInternship : Internship { }
    public class MinorInternship : Internship { }
    public class GraduationInternship : Internship { }
}

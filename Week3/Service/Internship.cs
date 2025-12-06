namespace SIS
{
    public class Period(int year, Semester semester)
    {
        public int Year { get; set; } = year;
        public Semester Semester { get; set; } = semester;
    }
    public class Internship
    {
        public int Id { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public string ProjectTitle { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public float? FinalGrade { get; set; }
        public InternshipStatus Status { get; set; } = InternshipStatus.OPEN;
        public int Capacity { get; set; } = 1;
        public List<Student> AssignedStudents { get; set; }
        private readonly List<Student> _assignedStudents = new();
        public IReadOnlyCollection<Student> AssignedStudents => _assignedStudents;
        
        public Period Period{get; set;}
        public Organization Organization{get; set;}
        private readonly List<ContactPerson> _contactPersons = new();
        public IReadOnlyCollection<ContactPerson> ContactPersons => _contactPerson;

        public void AddContactPerson(ContactPerson cp)
        {
            if(cp == null)
            {
                throw new ArgumentNullException(nameof(cp));
            }
            _contactPersons.Add(cp);
        }

        public void AssignStudents(Student s)
        {
            if(s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }
            if(_assignedStudents.Count >= Capacity)
            {
                throw new InvalidOperationException("No slot left!");
            }
            _assignedStudents.Add(s);
            if(_assignedStudents.Count >= Capacity)
            {
                Status = InternshipStatus.Assigned;
            }
        }

        public string Overview()
        {
            return $"{ProjectTitle} : {Status} \n {ShortDescription}";
        }
    }
    public class IntermediateInternship : Internship{}
    public class MinorInternship : Internship{}
    public class GraduationInternship : Internship{}
}
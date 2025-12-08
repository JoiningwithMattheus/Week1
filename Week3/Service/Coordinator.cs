namespace SIS
{
    public class Coordinator : IUser
    {
        public bool ProcessAssignment(Application application)
        {
            return false;
        }

        public void AssignStudentToInternship(Student student, Internship internship)
        {
            
        }

        public void MarkInternshipCompleted(Internship internship, float finalGrade)
        {
            
        }

        public bool AddOrganization(Organization organization)
        {
            return false;
        }

        public List<Organization> ListOrganizations()
        {
            
        }

        public void AddInternship(Internship internship, int orgID)
        {
            
        }

        public void WithdrawInterhsip(int internshipID)
        {
            
        }

        public List<Internship> ListInterships(Period period, InternshipCategory category)
        {
            
        }

        public List<ContactPerson> GetContactPersons(int internshipID)
        {
            
        }

        override public bool Login()
        {
            
        }

        override public void ShowMenu()
        {
            
        }
    }
}
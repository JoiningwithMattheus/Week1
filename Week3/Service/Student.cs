namespace SIS
{
    public class Student : IPerson, IUser
    {
        private uint studentNumber;
        private list<Application> applications;
        private Assignment assignment;

        public list<Internship> GetAvailableInternship(Period period, InternshipCategory category)
        {
            
        }

        public Application ApplyForInternship(Internship internship)
        {
            
        }

        public list<Application> GetApplications()
        {
            return applications;
        }

        public bool HasApplied(Period period)
        {
            
        }

        override bool Login()
        {

        }

        override void ShowMenu()
        {
            
        }
    }
}
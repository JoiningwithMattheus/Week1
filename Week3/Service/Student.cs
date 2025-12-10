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
            Console.WriteLine("Login as student successful.\n Opening Menu...");
            return true;

        }

        override void ShowMenu()
        {
            bool on = true;
            if (Login())
            {
                while (on)
                {
                    //take inputs, if quit input turn on to false
                }
            }
            else Console.Writeline("Login Failed.");
            
        }
    }
}
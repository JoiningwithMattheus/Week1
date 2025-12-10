namespace SIS
{
    public class Application
    {
        public int ApplicationID { get; set; }
        public DateTime SubmittedAt{ get; set; }
        public ApplicationStatus Status { get; set; }
        public string Motivation{ get; set; }
        
        public Student Student { get; set; }
        public Internship Internship { get; set; }  

        public string Summary()
        {
            return $"{ApplicationID} : {Status}\n{Internship?.Overview() ?? "N/A"}\n{Student?.GetFullName() ?? "N/A"}";
        }
    }
}
namespace SIS
{
    public abstract class Assignment
    {
        private string description;

        public string GetDescription()
        {
            return description;
        }
    }

    public class ResearchAssignment : Assignment
    {
        
    }

    public class EngineeringAssignment : Assignment
    {
        
    }

    public class MinorAssignment : Assignment
    {
        private Minor minor;  
    }
}
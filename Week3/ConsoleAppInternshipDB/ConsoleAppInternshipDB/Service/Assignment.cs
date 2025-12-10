namespace SIS
{
    public class Assignment
    {
        public string? description{get; set;}
    }

    public class ResearchAssignment : Assignment
    {
        
    }

    public class EngineeringAssignment : Assignment
    {
        
    }

    public class MinorAssignment : Assignment
    {
        public Minor? minor;  
    }
}
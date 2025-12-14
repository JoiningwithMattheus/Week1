namespace SIS
{
    public class Assignment
    {
        // PRIMARY KEY 
        public int AssignmentId { get; set; }

        public string Description { get; set; } = string.Empty;

        public string GetDescription() => Description;
    }

    public class ResearchAssignment : Assignment { }
    public class MinorAssignment : Assignment
    {
        public Minor? Minor { get; set; }
    }
    public class EngineeringAssignment : Assignment { }
}

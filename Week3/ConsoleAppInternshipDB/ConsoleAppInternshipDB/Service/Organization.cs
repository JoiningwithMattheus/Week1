namespace SIS;

public class Organization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Url { get; set; }

    public List<Internship> Internships { get; set; } = new();
    public int GetIdentifier()
    {
        return Id;
    }
}

public class Company : Organization
{

}

public class ResearchGroup : Organization
{

}

public class EducationInstitution : Organization
{

}
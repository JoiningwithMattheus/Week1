using Microsoft.EntityFrameworkCore;
namespace SIS;

public class SisDbContext : DbContext
{
    public DbSet<Coordinator> Coordinators { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Internship> Internships { get; set; }
    public DbSet<ContactPerson> ContactPersons { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Application> Applications { get; set; }

    public DbSet<IntermediateInternship> IntermediateInternships { get; set; }
    public DbSet<MinorInternship> MinorInternships { get; set; }
    public DbSet<GraduationInternship> GraduationInternships { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseMySql(
            "server=localhost;database=sisdb;user=root;password=your_password;",
            new MySqlServerVersion(new Version(8, 0, 22))
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Internship>()
            .HasMany(i => i.ContactPersons)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Organization>()
            .HasMany(o => o.Internships)
            .WithOne(i => i.Organization)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
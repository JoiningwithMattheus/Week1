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
        options.EnableSensitiveDataLogging();
        options.UseMySql(
            "server=localhost;database=sisdb;user=SISusers;password=1234567890;",
            new MySqlServerVersion(new Version(8, 0, 22))
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ContactPerson <-> Internship (explicit FK) — keep your existing config
        modelBuilder.Entity<ContactPerson>()
            .HasOne(cp => cp.Internship)
            .WithMany(i => i.ContactPersons)
            .HasForeignKey(cp => cp.InternshipId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Organization>()
            .HasMany(o => o.Internships)
            .WithOne(i => i.Organization)
            .OnDelete(DeleteBehavior.Cascade);

        // Make Period an owned/value object of Internship
        modelBuilder.Entity<Internship>().OwnsOne(i => i.Period, p =>
        {
            // column names in the Internship table:
            p.Property(pp => pp.Year).HasColumnName("PeriodYear");
            p.Property(pp => pp.Semester).HasColumnName("PeriodSemester");
        });
    }

}
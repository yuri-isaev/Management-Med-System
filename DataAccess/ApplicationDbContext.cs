using ManagementMedSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace ManagementMedSystem.DataAccess;

public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
  {}

  public DbSet<Patient> Patients { get; set; }
  public DbSet<Doctor> Doctors { get; set; }
  public DbSet<Area> Areas { get; set; }
  public DbSet<Specialization> Specializations { get; set; }
  public DbSet<Office> Offices { get; set; }
}

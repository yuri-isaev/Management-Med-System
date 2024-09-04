namespace ManagementMedSystem.Entities;

public class Doctor
{
  public int Id { get; set; }
  public string FullName { get; set; } = null!;
  public int OfficeId { get; set; }
  public Office Office { get; set; } = null!;
  public int SpecializationId { get; set; }
  public Specialization Specialization { get; set; } = null!;
  public int? AreaId { get; set; }
  public Area Area { get; set; } = null!;
}

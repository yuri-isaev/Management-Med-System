namespace ManagementMedSystem.Entities;

public class Patient
{
  public int Id { get; set; }
  public string LastName { get; set; } = null!;
  public string FirstName { get; set; } = null!;
  public string MiddleName { get; set; } = null!;
  public string Address { get; set; } = null!;
  public DateTime DateOfBirth { get; set; }
  public string Gender { get; set; } = null!;
  public int AreaId { get; set; }
  public Area Area { get; set; } = null!;
}

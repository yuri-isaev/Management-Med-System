namespace ManagementMedSystem.Dto;

public class PatientDto
{
  public int Id { get; set; }
  public string LastName { get; set; } = null!;
  public string FirstName { get; set; } = null!;
  public string MiddleName { get; set; } = null!;
  public string Address { get; set; } = null!;
  public DateTime DateOfBirth { get; set; } 
  public string Gender { get; set; } = null!;
  public int AreaId { get; set; } 
} 

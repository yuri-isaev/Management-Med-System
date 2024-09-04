using ManagementMedSystem.DataAccess;
using ManagementMedSystem.Dto;
using ManagementMedSystem.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementMedSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : BaseController
{
  public PatientsController(ApplicationDbContext context) : base(context)
  {}

  // GET: api/patients
  [HttpGet]
  public async Task<IActionResult> GetPatients(
    [FromQuery] string sortBy = "Id",
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
  {
    var query = Context.Patients.Include(p => p.Area).AsQueryable();

    // Определение порядка сортировки
    query = sortBy switch
    {
      "LastName" => query.OrderBy(p => p.LastName),
      "FirstName" => query.OrderBy(p => p.FirstName),
      "MiddleName" => query.OrderBy(p => p.MiddleName),
      "Address" => query.OrderBy(p => p.Address),
      "DateOfBirth" => query.OrderBy(p => p.DateOfBirth),
      "Gender" => query.OrderBy(p => p.Gender),
      _ => query.OrderBy(p => p.LastName),
    };

    // Постраничный вывод
    var pagedResult = await query
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .Select(p => new PatientListDto
      {
        LastName = p.LastName,
        FirstName = p.FirstName,
        MiddleName = p.MiddleName,
        Address = p.Address,
        DateOfBirth = p.DateOfBirth,
        Gender = p.Gender,
        AreaName = p.Area.Name
      })
      .ToListAsync();

    return Ok(pagedResult);
  }

  // GET: api/patients/{id}
  [HttpGet("{id}")]
  public async Task<IActionResult> GetPatientById(int id)
  {
    var patient = await Context.Patients
      .Where(p => p.Id == id)
      .Select(p => new PatientDto
      {
        AreaId = p.AreaId
      })
      .FirstOrDefaultAsync();

    if (patient == null) return NotFound();

    return Ok(patient);
  }

  // POST: api/patients
  [HttpPost]
  public async Task<IActionResult> CreatePatient([FromBody] PatientDto patientDto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var patient = new Patient
    {
      LastName = patientDto.LastName,
      FirstName = patientDto.FirstName,
      MiddleName = patientDto.MiddleName,
      Address = patientDto.Address,
      DateOfBirth = patientDto.DateOfBirth,
      Gender = patientDto.Gender,
      AreaId = patientDto.AreaId
    };

    Context.Patients.Add(patient);
    await Context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetPatientById), new {id = patient.Id}, patient);
  }

  // PUT: api/patients/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdatePatient(int id, [FromBody] PatientDto patientDto)
  {
    if (id != patientDto.Id) return BadRequest();

    var patient = await Context.Patients.FindAsync(id);
    
    if (patient == null) return NotFound();

    patient.LastName = patientDto.LastName;
    patient.FirstName = patientDto.FirstName;
    patient.MiddleName = patientDto.MiddleName;
    patient.Address = patientDto.Address;
    patient.DateOfBirth = patientDto.DateOfBirth;
    patient.Gender = patientDto.Gender;
    patient.AreaId = patientDto.AreaId;

    try
    {
      await Context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!EntityExists<Patient>(id))
      {
        return NotFound();
      }
      else
      {
        throw;
      }
    }

    return NoContent();
  }

  // DELETE: api/patients/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeletePatient(int id)
  {
    var patient = await Context.Patients.FindAsync(id);
    
    if (patient == null) return NotFound();

    Context.Patients.Remove(patient);
    await Context.SaveChangesAsync();

    return NoContent();
  }
}

using ManagementMedSystem.DataAccess;
using ManagementMedSystem.Dto;
using ManagementMedSystem.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementMedSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoctorsController : BaseController
{
  public DoctorsController(ApplicationDbContext context) : base(context)
  {}

  [HttpGet]
  public async Task<IActionResult> GetDoctors
  (
    [FromQuery] string sortBy = "Id",
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10
  )
  {
    // Основной запрос к базе данных
    var query = Context.Doctors
      .Include(d => d.Office)
      .Include(d => d.Specialization)
      .Include(d => d.Area)
      .AsQueryable();

    // Определение порядка сортировки
    query = sortBy switch
    {
      "FullName" => query.OrderBy(d => d.FullName),
      "OfficeNumber" => query.OrderBy(d => d.Office.Number),
      "SpecializationName" => query.OrderBy(d => d.Specialization.Name),
      "AreaNumber" => query.OrderBy(d => d.Area.Number),
      _ => query.OrderBy(d => d.Id)
    };

    // Постраничный вывод
    var pagedResult = await query
      .Skip((pageNumber - 1) * pageSize)
      .Take(pageSize)
      .Select(d => new DoctorListDto
      {
        FullName = d.FullName,
        OfficeNumber = d.Office.Number,
        SpecializationName = d.Specialization.Name,
        AreaNumber = d.Area.Number
      })
      .ToListAsync();

    return Ok(pagedResult);
  }

  // GET: api/doctors/{id}
  [HttpGet("{id}")]
  public async Task<IActionResult> GetDoctorById(int id)
  {
    var doctor = await Context.Doctors
      .Where(d => d.Id == id)
      .Select(d => new DoctorDto
      {
        OfficeId = d.OfficeId,
        SpecializationId = d.SpecializationId,
        AreaId = d.AreaId
      })
      .FirstOrDefaultAsync();

    if (doctor == null) return NotFound();

    return Ok(doctor);
  }

  // POST: api/doctors
  [HttpPost]
  public async Task<IActionResult> CreateDoctor([FromBody] DoctorDto doctorDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var doctor = new Doctor
    {
      OfficeId = doctorDto.OfficeId,
      SpecializationId = doctorDto.SpecializationId,
      AreaId = doctorDto.AreaId
    };

    Context.Doctors.Add(doctor);
    await Context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetDoctorById), new {id = doctor.Id}, doctor);
  }

  // PUT: api/doctors/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorDto doctorDto)
  {
    if (id != doctorDto.Id) return BadRequest();

    var doctor = await Context.Doctors.FindAsync(id);
    
    if (doctor == null) return NotFound();
    
    doctor.OfficeId = doctorDto.OfficeId;
    doctor.SpecializationId = doctorDto.SpecializationId;
    doctor.AreaId = doctorDto.AreaId;

    try
    {
      await Context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!EntityExists<Doctor>(id))
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

  // DELETE: api/doctors/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteDoctor(int id)
  {
    var doctor = await Context.Doctors.FindAsync(id);
    
    if (doctor == null) return NotFound();

    Context.Doctors.Remove(doctor);
    await Context.SaveChangesAsync();

    return NoContent();
  }
}

using ManagementMedSystem.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagementMedSystem.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
  protected readonly ApplicationDbContext Context;

  protected BaseController(ApplicationDbContext context)
  {
    Context = context;
  }

  protected bool EntityExists<TEntity>(int id) where TEntity : class
  {
    return Context.Set<TEntity>().Any(e => EF.Property<int>(e, "Id") == id);
  }
}

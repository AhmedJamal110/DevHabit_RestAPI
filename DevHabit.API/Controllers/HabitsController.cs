using Microsoft.AspNetCore.Mvc;
namespace DevHabit.API.Controllers;

[ApiController]
[Route("habits")]
public sealed class HabitsController(
    ApplicationDbContext _context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<Habit> habits = await _context.habits.ToListAsync();
        return Ok(habits);
    }
}

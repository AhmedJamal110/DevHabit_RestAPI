using DevHabit.API.Mapping;
using Microsoft.AspNetCore.JsonPatch;

namespace DevHabit.API.Controllers;

[ApiController]
[Route("habits")]
public sealed class HabitsController(ApplicationDbContext _context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {

        List<HabitDto> habitDtos = await _context.Habits
            .Select(HabitQueries.ProjectToDto())
            .ToListAsync();

        if(habitDtos.Count == 0)
        {
            return NotFound();
        }

        return Ok(habitDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {

        HabitDto? habitDto = await _context.Habits
            .Where(h => h.Id == id)
            .Select(HabitQueries.ProjectToDto())
            .FirstOrDefaultAsync();

        if (habitDto is null)
        {
            return NotFound();
        }   

        return Ok(habitDto);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateHabitRequest request)
    {

        Habit habit = request.ToHabitEntity();
        
        await _context.Habits.AddAsync(habit);
        await _context.SaveChangesAsync();

        var habitDto = habit.ToHabitDto();

        return CreatedAtAction(nameof(Get), new { id = habitDto.Id }, habitDto);  
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, UpdateHabitDto request)
    {
        Habit? habit = await _context.Habits.FirstOrDefaultAsync(h => h.Id == id);

        if(habit is null)
        {
            return NotFound();
        }

        habit.UpdateToEntity(request);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(string id , JsonPatchDocument<HabitDto> patchDocument)
    {
        Habit? habit = await _context.Habits.FirstOrDefaultAsync(h => h.Id == id);
       
        if (habit is null)
        {
            return NotFound();
        }

        var habitDto = habit.ToHabitDto();


        patchDocument.ApplyTo(habitDto,ModelState);


        if(TryValidateModel(habitDto))
        {
            return BadRequest(ModelState);
        }

        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        habit.Name = habitDto.Name;
        habit.Description = habitDto.Description;
        habit.Type = habitDto.Type;
        habit.Frequency.Type = habitDto.Frequency.Type;
        habit.Frequency.TimesPerPeriod = habitDto.Frequency.TimesPerPeriod;
        habit.Target.Value = habitDto.Target.Value; 
        habit.Target.Unit = habitDto.Target.Unit;
        habit.Status = habitDto.Status;
        habit.IsArchived = habitDto.IsArchived;
        habit.EndDate = habitDto.EndDate;
        habit.Milestone = habitDto.Milestone is null ? null : new Milestone
        {
            Target = habitDto.Milestone.Target,
            Current = habitDto.Milestone.Current
        };  

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        Habit? habit = await _context.Habits.FirstOrDefaultAsync(h => h.Id == id);
        
        if (habit is null)
        {
            //return StatusCode(StatusCodes.Status410Gone);  // IF use Soft Delete, then return 410 Gone

            return NotFound();
        }

        _context.Habits.Remove(habit);
        await _context.SaveChangesAsync();

        return NoContent();

    }
        
}

using DevHabit.API.Contracts.HabitTags;

namespace DevHabit.API.Controllers;

[ApiController]
[Route("habits/{habitId}/tags")]
public sealed class HabitTagsController(ApplicationDbContext _context) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> UpSert(string habitId , UpsertHabitTagsDto upsertHabitTagsDto)
    {
        Habit? habit = await _context.Habits
            .Include(h => h.tags)
            .FirstOrDefaultAsync(h => h.Id == habitId);
    
        if (habit is null)
        {
            return NotFound();
        }


        var tagIdsHasSet = habit.tags.Select(t => t.Id).ToHashSet();

        if (tagIdsHasSet.SetEquals(upsertHabitTagsDto.TagIds))
        {
            return NoContent();
        }

        List<string> exsitingTasgIDs = await _context.Tags
            .Where(t => upsertHabitTagsDto.TagIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        if(exsitingTasgIDs.Count != upsertHabitTagsDto.TagIds.Count)
        {
            return BadRequest();
        }



        habit.HabitTags.RemoveAll(ht => !exsitingTasgIDs.Contains(ht.TagId));

    }
}

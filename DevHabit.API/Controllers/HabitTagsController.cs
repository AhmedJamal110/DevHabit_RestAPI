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
            .Include(h => h.HabitTags)
            .FirstOrDefaultAsync(h => h.Id == habitId);

        if (habit is null)
        {
            return NotFound();
        }

        var currentTagIds = habit.HabitTags.Select(ht => ht.TagId).ToHashSet();
        
        if (currentTagIds.SetEquals(upsertHabitTagsDto.TagIds))
        {
            return NoContent();
        }


        List<string> exsistingTagsIds = await _context.Tags
            .Where(t => upsertHabitTagsDto.TagIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        if (exsistingTagsIds.Count != upsertHabitTagsDto.TagIds.Count)
        {
            return BadRequest("Some tags IDS do not exist.");
        }

    
        habit.HabitTags.RemoveAll(ht => !upsertHabitTagsDto.TagIds.Contains(ht.TagId));

        string[] tagIdsToAdd = [.. upsertHabitTagsDto.TagIds.Except(currentTagIds)];

        habit.HabitTags.AddRange(tagIdsToAdd.Select(tagId => new HabitTag
        {
            HabitId = habitId,
            TagId = tagId,
            CreatedAtUtc = DateTime.UtcNow
        }));  

        await _context.SaveChangesAsync();


        return Ok();

    }
    
    [HttpDelete("{tagId}")]
    public async Task<IActionResult> Delete(string habitId , string tagId)
    {
        HabitTag? habitTag = await _context.HabitTags
            .FirstOrDefaultAsync(h => h.HabitId == habitId && h.TagId == tagId);

        if (habitTag is null)
        {
            return NotFound();
        }

        _context.HabitTags.Remove(habitTag);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    

}

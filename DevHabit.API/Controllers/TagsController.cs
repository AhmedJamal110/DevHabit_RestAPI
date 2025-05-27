using DevHabit.API.Contracts.Tags;
using DevHabit.API.Mapping;

namespace DevHabit.API.Controllers;

[ApiController]
[Route("tags")]
public sealed class TagsController(ApplicationDbContext _context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {

        List<TagDto> tagDtos = await _context.Tags
            .Select(tag => tag.ToTagDto())
            .ToListAsync();

        if (tagDtos.Count == 0)
        {

            return NotFound();
        }

        return Ok(tagDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {

        Tag? tag = await _context.Tags.FindAsync(id);
      
        if (tag is null)
        {
            return NotFound();

        }

        var tagDto = tag.ToTagDto();

        return Ok(tagDto);
    }

    [HttpPost]
    public async Task<IActionResult> Post(TagDto request)
    {
        var tag = new Tag
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Description = request.Description,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();

        return Ok(tag.ToTagDto());
    }
}

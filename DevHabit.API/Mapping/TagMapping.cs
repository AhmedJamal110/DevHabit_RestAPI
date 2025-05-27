using DevHabit.API.Contracts.Tags;

namespace DevHabit.API.Mapping;

public static class TagMapping
{
    public static Tag ToTagEntity(this CreateTagRequest request)
    {
        Tag tag = new()
        {
            Id = $"h_{Guid.CreateVersion7()}",
            Name = request.Name,
            Description = request.Description,
            CreatedAtUtc = DateTime.UtcNow,
        };

        return tag;
    }

    public static TagDto ToTagDto(this Tag tag)
    {
        return new TagDto
        {
            Id = tag.Id,
            Name = tag.Name,
            Description = tag.Description,
            CreatedAtUtc = tag.CreatedAtUtc,
            UpdatedAtUtc = tag.UpdatedAtUtc
        };
    }

    //public static void UpdateToEntity(this Tag habit, UpdateTagDto dto)
    //{
    //    // Update basic properties
    //    habit.Name = dto.Name;
    //    habit.Description = dto.Description;
    //    habit.Type = dto.Type;
    //    habit.EndDate = dto.EndDate;

    //    // Update frequency (assuming it's immutable, create new instance)
    //    habit.Frequency = new Frequency
    //    {
    //        Type = dto.Frequency.Type,
    //        TimesPerPeriod = dto.Frequency.TimesPerPeriod
    //    };

    //    // Update target
    //    habit.Target = new Target
    //    {
    //        Value = dto.Target.Value,
    //        Unit = dto.Target.Unit
    //    };

    //    // Update milestone if provided
    //    if (dto.Milestone != null)
    //    {
    //        habit.Milestone ??= new Milestone(); // Create new if doesn't exist
    //        habit.Milestone.Target = dto.Milestone.Target;
    //        // Note: We don't update Milestone.Current from DTO to preserve progress
    //    }

    //    habit.UpdatedAtUtc = DateTime.UtcNow;
    //}

}

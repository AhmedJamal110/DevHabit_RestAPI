namespace DevHabit.API.Contracts.HabitTags;

public sealed record UpsertHabitTagsDto
{
    public required List<string> TagIds { get; set; }
}

namespace DevHabit.API.Contracts.Tags;

public sealed record CreateTagRequest
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    
}

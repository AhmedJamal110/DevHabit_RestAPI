namespace DevHabit.API.Contracts.Target;

public sealed record TargetDto
{
    public required int Value { get; set; }
    public required string Unit { get; set; }
}

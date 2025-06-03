namespace DevHabit.API.Contracts.Habits;

public sealed record HabitQueryParamter
{
    [ FromQuery(Name ="q")]
    public string? Search { get; set; }
    public HabitType? Type  { get; init; }
    public HabitStatus? Status { get; init; }
    public string? Sort { get; init; }

    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; } = 10;
}

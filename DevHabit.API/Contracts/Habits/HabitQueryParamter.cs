namespace DevHabit.API.Contracts.Habits;

public sealed record HabitQueryParamter
{
    [ FromQuery(Name ="q")]
    public string? Search { get; set; }
    public HabitType? Type  { get; init; }
    public HabitStatus? Status { get; init; }

}

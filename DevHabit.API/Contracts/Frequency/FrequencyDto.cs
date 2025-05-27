namespace DevHabit.API.Contracts.Frequency;

public sealed record FrequencyDto
{
    public required FrequencyType Type { get; set; }
    public required int TimesPerPeriod { get; set; }
}

using FluentValidation;

namespace DevHabit.API.Contracts.Habits;

public sealed class CreateHabitRequestValidator : AbstractValidator<CreateHabitRequest>
{

    private static readonly string[] AllowedUnits =
    [
         "minutes", "hours", "steps", "km", "pages", "books", "tasks", "sessions"
    ];


    private static readonly string[] AllowedUnitsForBinaryHabits =
    [
       "sessions", "tasks"
    ];

    public CreateHabitRequestValidator()
    {
        RuleFor(h => h.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100)
                .WithMessage("Habit Must be between 3 and 100 characters long.");


        RuleFor(h => h.Description)
            .MaximumLength(300)
            .When(h => h.Description is not null)
            .WithMessage("Habit not exceed 500 characters long.");

        RuleFor(h => h.Type)
            .IsInEnum()
            .WithMessage("Habit Type must be a valid enum value.");


        RuleFor(h => h.Frequency.Type)
            .IsInEnum()
            .WithMessage("Frequency Type must be a valid enum value.");


        RuleFor(h => h.Target.Value)
            .GreaterThan(0)
            .WithMessage("Target Value must be greater than 0.");

        RuleFor(h => h.Target.Unit)
            .NotEmpty()
            .Must((dto, unit) => IsTargetUnitCompatabileWithHabitType(dto.Type, unit))
            .WithMessage("Target Unit must be compatible with the Habit Type.");
    }

    private static bool IsTargetUnitCompatabileWithHabitType(HabitType habitType, string unit)
    {
        string normalizedUnit = unit.ToLowerInvariant();
    
        return habitType switch
        {
            HabitType.Binary => AllowedUnitsForBinaryHabits.Contains(normalizedUnit),
            HabitType.Measurable => AllowedUnits.Contains(normalizedUnit),
            _ => false
        };

    }

   
}


namespace DevHabit.API.Database.Configurations;

public sealed class HabitTagConfiguration : IEntityTypeConfiguration<HabitTag>
{
    public void Configure(EntityTypeBuilder<HabitTag> builder)
    {
        builder.HasKey(ht => new { ht.HabitId, ht.TagId });

        builder.HasOne<Habit>()
            .WithMany(h => h.HabitTags)
            .HasForeignKey(h => h.HabitId);
                

        builder.HasOne<Tag>()
            .WithMany()
            .HasForeignKey(t => t.TagId);


    }
}

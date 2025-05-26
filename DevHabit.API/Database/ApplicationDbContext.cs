using System.Reflection;

namespace DevHabit.API.Database;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) :DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema(Schemas.Application)
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }


    public DbSet<Habit> habits { get; set; }

}

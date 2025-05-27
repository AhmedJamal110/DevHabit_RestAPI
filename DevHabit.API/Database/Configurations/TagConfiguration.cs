namespace DevHabit.API.Database.Configurations;
public sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasIndex(t => new { t.Name }) 
            .IsUnique()
            .HasDatabaseName("IX_Tag_Name");


        builder.Property(t => t.Id)
            .HasMaxLength(500);


        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

    }
}


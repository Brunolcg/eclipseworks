namespace Eclipseworks.Infrastructure.Persistence.EntityConfigurations;

public class ProjectEntityTypeConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder
            .ToTable("Projects");
        
        builder
            .HasKey(p => p.Id);
        
        builder
            .Property(p => p.Id)
            .ValueGeneratedNever();

        builder
            .HasMany(p => p.Tasks)
            .WithOne(pt => pt.Project)
            .HasForeignKey(p => p.ProjectId);

        builder
            .Property(p => p.Name)
            .IsRequired();
    }
}

namespace Eclipseworks.Infrastructure.Persistence.EntityConfigurations;

public class ProjectTaskEntityTypeConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder
            .ToTable("ProjectTasks");
        
        builder
            .HasKey(p => p.Id);
        
        builder
            .Property(p => p.Id)
            .ValueGeneratedNever();
        
        builder
            .Property(p => p.Title)
            .IsRequired();
        
        builder
            .Property(p => p.Description)
            .IsRequired();
        
        builder
            .Property(p => p.Priority)
            .IsRequired();
        
        builder
            .Property(p => p.Status)
            .IsRequired();
        
        builder
            .Property(p => p.DueDate)
            .IsRequired();
        
        builder
            .HasMany(p => p.Comments)
            .WithOne(pt => pt.ProjectTask)
            .HasForeignKey(p => p.ProjectTaskId);
        
        builder
            .HasOne(p => p.Responsible)
            .WithMany()
            .HasForeignKey(p => p.ResponsibleId);
    }
}

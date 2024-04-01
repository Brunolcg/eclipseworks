namespace Eclipseworks.Infrastructure.Persistence.EntityConfigurations;

public class ProjectTaskCommentEntityTypeConfiguration : IEntityTypeConfiguration<ProjectTaskComment>
{
    public void Configure(EntityTypeBuilder<ProjectTaskComment> builder)
    {
        builder
            .ToTable("ProjectTaskComments");
        
        builder
            .HasKey(p => p.Id);
        
        builder
            .Property(p => p.Id)
            .ValueGeneratedNever();
        
        builder
            .Property(p => p.Description)
            .IsRequired();
        
        builder
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId);
    }
}

using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();
        builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");

        builder.HasIndex(x => x.EmployeeId);

        builder.HasOne(x => x.Employee)
            .WithMany(y => y.Projects)
            .HasForeignKey(x => x.EmployeeId);
    }
}
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.UserName).IsRequired();
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(120);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(120);
        builder.Property(x => x.EmployeeNumber).IsRequired().HasMaxLength(50);

        builder.HasIndex(x => x.DepartmentId);

        builder.HasOne(x => x.Department)
            .WithMany(y => y.Employees)
            .HasForeignKey(x => x.DepartmentId);

        

    }
}
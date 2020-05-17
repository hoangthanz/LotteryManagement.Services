using LotteryManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LotteryManagement.Data.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {


        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.FunctionId).IsRequired();

            builder.HasOne(x => x.AppUser).WithMany(x => x.Permissions).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Function).WithMany(x => x.Permissions).HasForeignKey(x => x.FunctionId);
        }
    }
}

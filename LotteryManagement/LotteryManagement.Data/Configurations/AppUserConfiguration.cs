using LotteryManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LotteryManagement.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");
            builder.Property(x => x.NickName).IsRequired();
            builder.Property(x => x.TransactionPassword).IsRequired();

            builder.HasOne(x => x.RootUser).WithMany(x => x.RefUsers).HasForeignKey(x => x.RootUserId);
        }
    }
}

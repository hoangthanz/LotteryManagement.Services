using LotteryManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LotteryManagement.Data.Configurations
{
    public class BankCardConfiguration : IEntityTypeConfiguration<BankCard>
    {

        public void Configure(EntityTypeBuilder<BankCard> builder)
        {
            builder.ToTable("BankCards");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.BankName).IsRequired();
            builder.Property(x => x.BankBranch).IsRequired();
            builder.Property(x => x.BankAccountNumber).IsRequired();

            builder.HasOne(x => x.AppUser).WithMany(x => x.BankCards).HasForeignKey(x => x.UserId);
        }
    }
}

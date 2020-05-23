using LotteryManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LotteryManagement.Data.Configurations
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("Wallets");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Coin).IsRequired();

            builder.Property(x => x.PromotionCoin).IsRequired();

            builder.Property(x => x.PendingCoin).IsRequired();

        }
    }
}

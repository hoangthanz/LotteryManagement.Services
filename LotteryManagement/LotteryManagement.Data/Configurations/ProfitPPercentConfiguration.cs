using LotteryManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LotteryManagement.Data.Configurations
{
    public class ProfitPPercentConfiguration : IEntityTypeConfiguration<ProfitPercent>
    {
        public void Configure(EntityTypeBuilder<ProfitPercent> builder)
        {
            builder.ToTable("ProfitPercents");
            builder.HasKey(x => x.Id);

            //

            builder.Property(x => x.Lo2So).IsRequired();
            builder.Property(x => x.Lo2SoDau).IsRequired();
            builder.Property(x => x.Lo2So1K).IsRequired();
            builder.Property(x => x.Lo3So).IsRequired();
            builder.Property(x => x.Lo4So).IsRequired();

            builder.Property(x => x.Xien2).IsRequired();
            builder.Property(x => x.Xien3).IsRequired();
            builder.Property(x => x.Xien4).IsRequired();

            builder.Property(x => x.DeDacBiet).IsRequired();
            builder.Property(x => x.DeDauDacBiet).IsRequired();
            builder.Property(x => x.DeGiai7).IsRequired();
            builder.Property(x => x.DeGiaiNhat).IsRequired();

            builder.Property(x => x.Dau).IsRequired();
            builder.Property(x => x.Duoi).IsRequired();

            builder.Property(x => x.Cang3).IsRequired();
            builder.Property(x => x.Cang4).IsRequired();

            builder.Property(x => x.TruotXien4).IsRequired();
            builder.Property(x => x.TruotXien8).IsRequired();
            builder.Property(x => x.TruotXien10).IsRequired();
        }
    }
}

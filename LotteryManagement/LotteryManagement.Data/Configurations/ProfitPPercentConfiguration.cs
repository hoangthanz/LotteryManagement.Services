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


            // Pre
            builder.Property(x => x.Lo2SoPrevious).IsRequired();
            builder.Property(x => x.Lo2SoDauPrevious).IsRequired();
            builder.Property(x => x.Lo2So1KPrevious).IsRequired();
            builder.Property(x => x.Lo3SoPrevious).IsRequired();
            builder.Property(x => x.Lo4SoPrevious).IsRequired();

            builder.Property(x => x.Xien2Previous).IsRequired();
            builder.Property(x => x.Xien3Previous).IsRequired();
            builder.Property(x => x.Xien4Previous).IsRequired();

            builder.Property(x => x.DeDacBietPrevious).IsRequired();
            builder.Property(x => x.DeDauDacBietPrevious).IsRequired();
            builder.Property(x => x.DeGiai7Previous).IsRequired();
            builder.Property(x => x.DeGiaiNhatPrevious).IsRequired();

            builder.Property(x => x.DauPrevious).IsRequired();
            builder.Property(x => x.DuoiPrevious).IsRequired();

            builder.Property(x => x.Cang3Previous).IsRequired();
            builder.Property(x => x.Cang4Previous).IsRequired();

            builder.Property(x => x.TruotXien4Previous).IsRequired();
            builder.Property(x => x.TruotXien8Previous).IsRequired();
            builder.Property(x => x.TruotXien10Previous).IsRequired();

            //After

            builder.Property(x => x.Lo2SoAfter).IsRequired();
            builder.Property(x => x.Lo2SoDauAfter).IsRequired();
            builder.Property(x => x.Lo2So1KAfter).IsRequired();
            builder.Property(x => x.Lo3SoAfter).IsRequired();
            builder.Property(x => x.Lo4SoAfter).IsRequired();

            builder.Property(x => x.Xien2After).IsRequired();
            builder.Property(x => x.Xien3After).IsRequired();
            builder.Property(x => x.Xien4After).IsRequired();

            builder.Property(x => x.DeDacBietAfter).IsRequired();
            builder.Property(x => x.DeDauDacBietAfter).IsRequired();
            builder.Property(x => x.DeGiai7After).IsRequired();
            builder.Property(x => x.DeGiaiNhatAfter).IsRequired();

            builder.Property(x => x.DauAfter).IsRequired();
            builder.Property(x => x.DuoiAfter).IsRequired();

            builder.Property(x => x.Cang3After).IsRequired();
            builder.Property(x => x.Cang4After).IsRequired();

            builder.Property(x => x.TruotXien4After).IsRequired();
            builder.Property(x => x.TruotXien8After).IsRequired();
            builder.Property(x => x.TruotXien10After).IsRequired();
        }
    }
}

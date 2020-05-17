using LotteryManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LotteryManagement.Data.Configurations
{
    public class De_LottoConfiguration : IEntityTypeConfiguration<De_Lotto>
    {
        public void Configure(EntityTypeBuilder<De_Lotto> builder)
        {
            builder.ToTable("De_Lottos");
            builder.HasKey(x => x.Id);


            builder.Property(x => x.CurrentRate).IsRequired();
            builder.Property(x => x.beginRate).IsRequired();
            builder.Property(x => x.endRate).IsRequired();

            builder.Property(x => x.Value).IsRequired();
            builder.Property(x => x.De_LottoStatus).IsRequired();
            builder.Property(x => x.RegionStatus).IsRequired();

            builder.HasOne(x => x.Ticket).WithMany(x => x.De_Lottos).HasForeignKey(x => x.TicketId);
        }
    }
}

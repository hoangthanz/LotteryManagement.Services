using LotteryManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LotteryManagement.Data.Configurations
{
    public class Cang_LottoConfiguration : IEntityTypeConfiguration<Cang_Lotto>
    {
        public void Configure(EntityTypeBuilder<Cang_Lotto> builder)
        {
            builder.ToTable("Cang_Lottos");
            builder.HasKey(x => x.Id);


            builder.Property(x => x.CurrentRate).IsRequired();
            builder.Property(x => x.BeginRate).IsRequired();
            builder.Property(x => x.EndRate).IsRequired();

            builder.Property(x => x.Value).IsRequired();
            builder.Property(x => x.Cang_LottoStatus).IsRequired();
            builder.Property(x => x.RegionStatus).IsRequired();

            builder.HasOne(x => x.Ticket).WithMany(x => x.Cang_Lottos).HasForeignKey(x => x.TicketId);
        }
    }
 
}

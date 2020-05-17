using LotteryManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LotteryManagement.Data.Configurations
{
    public class Xien_LottoConfiguration : IEntityTypeConfiguration<Xien_Lotto>
    {
        public void Configure(EntityTypeBuilder<Xien_Lotto> builder)
        {
            builder.ToTable("Xien_Lottos");
            builder.HasKey(x => x.Id);
           

            builder.Property(x => x.Value).IsRequired(true);

            builder.HasOne(x => x.Ticket).WithMany(x => x.Xien_Lottos).HasForeignKey(x => x.TicketId);
        }
    }
}

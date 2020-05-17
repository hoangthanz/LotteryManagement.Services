using LotteryManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LotteryManagement.Data.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {


        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets");
            builder.HasKey(x => x.Id);

        
            builder.Property(x => x.UserId).IsRequired();


            builder.HasOne(x => x.AppUser).WithMany(x => x.Tickets).HasForeignKey(x => x.UserId);
         
        }
    }
}

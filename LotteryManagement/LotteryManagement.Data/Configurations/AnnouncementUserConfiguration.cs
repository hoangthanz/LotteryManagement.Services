using LotteryManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LotteryManagement.Data.Configurations
{
    public class AnnouncementUserConfiguration : IEntityTypeConfiguration<AnnouncementUser>
    {


        public void Configure(EntityTypeBuilder<AnnouncementUser> builder)
        {
            builder.ToTable("AnnouncementUsers");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AnnouncementId).IsRequired();
            builder.Property(x => x.UserId).IsRequired();


            builder.HasOne(x => x.AppUser).WithMany(x => x.AnnouncementUsers).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.Announcement).WithMany(x => x.AnnouncementUsers).HasForeignKey(x => x.AnnouncementId);
        }
    }
}

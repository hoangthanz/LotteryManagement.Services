using LotteryManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LotteryManagement.Data.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {

        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("ContactDetails");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);

            builder.Property(x => x.Phone).HasMaxLength(50);

            builder.Property(x => x.Email).HasMaxLength(250);

            builder.Property(x => x.Website).HasMaxLength(250);


        }
    }
}

using LotteryManagement.Data.Configurations;
using LotteryManagement.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace LotteryManagement.Data.EF
{
    public class LotteryManageDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public LotteryManageDbContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configure using Fluent API
            _ = builder.ApplyConfiguration(new FeedbackConfiguration());

            builder.ApplyConfiguration(new AnnouncementConfiguration());
            builder.ApplyConfiguration(new AnnouncementUserConfiguration());




            builder.ApplyConfiguration(new AppConfigConfiguration());
            builder.ApplyConfiguration(new AppUserConfiguration());
            builder.ApplyConfiguration(new AppRoleConfiguration());



            builder.ApplyConfiguration(new Bao_LottoConfiguration());
            builder.ApplyConfiguration(new Cang_LottoConfiguration());
            builder.ApplyConfiguration(new De_LottoConfiguration());
            builder.ApplyConfiguration(new Xien_LottoConfiguration());
            
            builder.ApplyConfiguration(new MessageConfiguration());
            builder.ApplyConfiguration(new FunctionConfiguration());

            builder.ApplyConfiguration(new OperationHistoryConfiguration());
            builder.ApplyConfiguration(new PermissionConfiguration());
            builder.ApplyConfiguration(new ProfitPPercentConfiguration());

            builder.ApplyConfiguration(new PromotionConfiguration());
            builder.ApplyConfiguration(new TicketConfiguration());
            builder.ApplyConfiguration(new TransactionHistoryConfiguration());
            builder.ApplyConfiguration(new WalletConfiguration());


            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);


            // base.OnModelCreating(builder);
        }
        public DbSet<Announcement> Announcements { get; set; }

        public DbSet<AnnouncementUser> AnnouncementUsers { get; set; }
        public DbSet<AppConfig> AppConfigs { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<Bao_Lotto> Bao_Lottos { get; set; }

        public DbSet<Cang_Lotto> Cang_Lottos { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<De_Lotto> De_Lottos { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Function> Functions { get; set; }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<OperationHistory> OperationHistories { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<ProfitPercent> ProfitPercents { get; set; }

        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TransactionHistory> TransactionHistories { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<Xien_Lotto> Xien_Lottos { get; set; }

    }
}

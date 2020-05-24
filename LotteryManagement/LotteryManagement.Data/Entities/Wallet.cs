using LotteryManagement.Data.Enums;
using LotteryManagement.Data.Interfaces;
using LotteryManagement.Infrastructure.SharedKernel;
using System;

namespace LotteryManagement.Data.Entities
{
    public class Wallet : DomainEntity<string>, ISwitchable, IDateTracking
    {
        public string WalletId { get; set; }

        public string UserId { get; set; }

        public AppUser AppUser { get; set; }

        
        public double Coin { get; set; }

        public double PromotionCoin { get; set; }

        public double PendingCoin { get; set; }

        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }
        public Status Status { set; get; }
    }
}

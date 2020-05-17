using LotteryManagement.Data.Enums;
using LotteryManagement.Data.Interfaces;
using LotteryManagement.Infrastructure.SharedKernel;
using System;

namespace LotteryManagement.Data.Entities
{
    public class Wallet : DomainEntity<string>, ISwitchable, IDateTracking
    {
        public string WalletId { get; set; }

        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }

        public double Total { get; set; }




        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }
        public Status Status { set; get; }
    }
}

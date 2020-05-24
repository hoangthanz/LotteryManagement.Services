using LotteryManagement.Data.Enums;
using System;

namespace LotteryManagement.Application.ViewModels
{
    public class WalletViewModel
    {
        public string Id { get; set; }
        public string WalletId { get; set; }

        public string UserId { get; set; }

        public AppUserViewModel AppUser { get; set; }


        public double Coin { get; set; }

        public double PromotionCoin { get; set; }

        public double PendingCoin { get; set; }

        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }
        public Status Status { set; get; }
    }
}

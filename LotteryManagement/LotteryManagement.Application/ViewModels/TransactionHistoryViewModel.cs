using LotteryManagement.Data.Enums;
using System;

namespace LotteryManagement.Application.ViewModels
{
    public class TransactionHistoryViewModel
    {
        public string Id { get; set; }
        public string Content { get; set; }

        public string UserId { get; set; }

        public TransactionHistoryType TransactionHistoryType { get; set; }
        public BillStatus BillStatus { get; set; }

        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }
        public Status Status { set; get; }
        public double Coin { get; set; }

        public AppUserViewModel AppUser { get; set; }
    }
}

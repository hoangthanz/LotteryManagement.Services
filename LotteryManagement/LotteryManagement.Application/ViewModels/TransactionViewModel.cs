using LotteryManagement.Data.Enums;
using System;

namespace LotteryManagement.Application.ViewModels
{
    public class TransactionViewModel
    {
        public string Id { get; set; }
        public string Content { get; set; }

        public Guid UserId { get; set; }

        public TransactionType TransactionType { get; set; }

        public BillStatus BillStatus { get; set; }

        public DateTime DateCreated { set; get; }

        public DateTime? DateModified { set; get; }

        public Status Status { set; get; }

        public double Coin { get; set; }

        public bool IsVerified { get; set; }

        public AppUserViewModel AppUserViewModel { get; set; }
    }
}

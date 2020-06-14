using LotteryManagement.Data.Enums;
using LotteryManagement.Data.Interfaces;
using LotteryManagement.Infrastructure.SharedKernel;
using System;

namespace LotteryManagement.Data.Entities
{
    public class TransactionHistory : DomainEntity<string>, ISwitchable, IDateTracking
    {

        public string Content { get; set; }

        public string UserId { get; set; }

        public TransactionHistoryType TransactionHistoryType { get; set; }
        public BillStatus BillStatus { get; set; }

        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }
        public Status Status { set; get; }
        public double Coin { get; set; }

        public string BankCardId { get; set; }

        public string OwnerBankId { get; set; }

    }
}

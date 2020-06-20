using System;

namespace LotteryManagement.Application.ViewModels.Conditions
{
    public class TransactionHistoryCondition
    {
        public int? TransactionType { get; set; }
        public int? BillStatus { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string UserId { get; set; }
    }
}

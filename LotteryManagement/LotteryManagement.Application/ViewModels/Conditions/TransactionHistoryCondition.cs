using System;
using System.Collections.Generic;
using System.Text;

namespace LotteryManagement.Application.ViewModels.Conditions
{
    public class TransactionHistoryCondition
    {
        public int? TransactionType { get; set; }
        public int? BillStatus { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}

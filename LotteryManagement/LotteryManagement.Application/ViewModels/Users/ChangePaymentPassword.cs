using System;
using System.Collections.Generic;
using System.Text;

namespace LotteryManagement.Application.ViewModels.Users
{
    public class ChangePaymentPassword
    {
        public string PaymentPasswork { get; set; }
        public string NewPaymentPasswork { get; set; }

        public string ConfirmNewPaymentPasswork { get; set; }
    }
}

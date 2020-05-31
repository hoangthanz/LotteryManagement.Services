using System;
using System.Collections.Generic;
using System.Text;

namespace LotteryManagement.Application.ViewModels.Users
{
    public class ChangePassword
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}

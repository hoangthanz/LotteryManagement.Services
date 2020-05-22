using System;

namespace LotteryManagement.Application.ViewModels
{
    public class AppUserViewModel
    {
        public Guid? Id { set; get; }
     
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Guid? LinkRef { get; set; }

        public string NickName { get; set; }

        public string TransactionPassword { get; set; }

        public string Avatar { get; set; }

        /* Đối chiếu mã ví khi nạpk tiền */
        public string WalletId { get; set; }
    }
}

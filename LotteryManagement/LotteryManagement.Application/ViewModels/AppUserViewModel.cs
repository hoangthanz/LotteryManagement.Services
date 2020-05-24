using System;

namespace LotteryManagement.Application.ViewModels
{
    public class AppUserViewModel
    {
        public string Id { set; get; }

        public string UserName { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Email { get; set; }

        public string RootUserId { get; set; }

        public string NickName { get; set; }

        public string TransactionPassword { get; set; }

        public string Avatar { get; set; }

        /* Đối chiếu mã ví khi nạpk tiền */
        public string WalletId { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }
}

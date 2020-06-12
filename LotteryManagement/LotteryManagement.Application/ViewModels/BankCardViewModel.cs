using LotteryManagement.Data.Enums;
using System;

namespace LotteryManagement.Application.ViewModels
{
    public class BankCardViewModel
    {
        public string Id { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }

        public string FullNameOwner { get; set; }

        public string BankAccountNumber { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public Status Status { get; set; }

        public string UserId { get; set; }

        public AppUserViewModel AppUser { get; set; }
    }
}

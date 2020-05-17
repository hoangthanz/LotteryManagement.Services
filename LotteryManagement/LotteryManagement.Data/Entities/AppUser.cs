using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace LotteryManagement.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string NickName { get; set; }

        public string TransactionPassword { get; set; }

        public string Avatar { get; set; }

        /* Đối chiếu mã ví khi nạpk tiền */
        public string WalletId { get; set; }

        public List<AnnouncementUser> AnnouncementUsers { get; set; }

        public List<Message> Messages { get; set; }

        public List<Wallet> Wallets { get; set; }

        public List<Permission> Permissions { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}

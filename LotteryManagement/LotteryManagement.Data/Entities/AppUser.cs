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

        public List<AnnouncementUser> AnnouncementUsers { get; set; }
    }
}

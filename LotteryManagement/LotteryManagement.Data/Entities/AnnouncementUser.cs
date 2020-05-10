using LotteryManagement.Infrastructure.SharedKernel;
using System;

namespace LotteryManagement.Data.Entities
{
    public class AnnouncementUser : DomainEntity<int>
    {
        public AnnouncementUser() { }
        public AnnouncementUser(string announcementId, Guid userId, bool? hasRead)
        {
            AnnouncementId = announcementId;
            UserId = userId;
            HasRead = hasRead;
        }


        public string AnnouncementId { get; set; }

        public Guid UserId { get; set; }

        public bool? HasRead { get; set; }

        public AppUser AppUser { get; set; }

        public Announcement Announcement { get; set; }
    }
}

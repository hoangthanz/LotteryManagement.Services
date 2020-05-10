using LotteryManagement.Data.Enums;
using LotteryManagement.Data.Interfaces;
using LotteryManagement.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;

namespace LotteryManagement.Data.Entities
{
    public class Announcement : DomainEntity<string>, ISwitchable, IDateTracking
    {

        public Announcement(string title, string content, Guid userId, Status status)
        {
            Title = title;
            Content = content;
            UserId = userId;
            Status = status;
        }


        public string Title { set; get; }


        public string Content { set; get; }

        public Guid UserId { set; get; }


        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public Status Status { set; get; }

        public List<AnnouncementUser> AnnouncementUsers { get; set; }
    }
}

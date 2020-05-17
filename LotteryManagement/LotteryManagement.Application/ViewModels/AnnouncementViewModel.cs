using LotteryManagement.Data.Enums;
using System;

namespace LotteryManagement.Application.ViewModels
{
    public class AnnouncementViewModel
    {
        public string Id { get; set; }

        public string Title { set; get; }

        public string Content { set; get; }

        public Guid UserId { set; get; }

        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }
        public Status Status { set; get; }
    }
}

using LotteryManagement.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LotteryManagement.Application.ViewModels
{
    public class FeedbackViewModel
    {
        public int Id { get; set; }

        public string Name { set; get; }

        public string Email { set; get; }

        public string Message { set; get; }

        public Status Status { set; get; }

        public DateTime DateCreated { set; get; }

        public DateTime? DateModified { get; set; }
    }
}

﻿using LotteryManagement.Data.Enums;
using LotteryManagement.Data.Interfaces;
using LotteryManagement.Infrastructure.SharedKernel;
using System;

namespace LotteryManagement.Data.Entities
{
    public class Message : DomainEntity<string>, ISwitchable, IDateTracking
    {
        public string Content { get; set; }

        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }

        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }
        public Status Status { set; get; }
    }
}
using LotteryManagement.Data.Enums;
using LotteryManagement.Data.Interfaces;
using LotteryManagement.Infrastructure.SharedKernel;
using System;

namespace LotteryManagement.Data.Entities
{
    public class Permission : DomainEntity<string>, ISwitchable,IDateTracking
    {
        public bool hasRead { get; set; }

        public bool hasCreate { get; set; }
        public bool hasUpdate { get; set; }
        public bool hasDelete { get; set; }

        public string FunctionId { get; set; }
        public Function Function { get; set; }

        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }


        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }
        public Status Status { set; get; }
    }
}

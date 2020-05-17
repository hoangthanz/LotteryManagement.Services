using LotteryManagement.Data.Enums;
using LotteryManagement.Data.Interfaces;
using LotteryManagement.Infrastructure.SharedKernel;
using System.Collections.Generic;

namespace LotteryManagement.Data.Entities
{
    public class Function : DomainEntity<string>, ISwitchable
    {
        public string Name { get; set; }
        public string Code { get; set; }

        public Status Status { get; set; }


        public List<Permission> Permissions { get; set; }
    }
}

using LotteryManagement.Data.Enums;
using LotteryManagement.Data.Interfaces;
using LotteryManagement.Infrastructure.SharedKernel;
using System;

namespace LotteryManagement.Data.Entities
{
    public class De_Lotto: DomainEntity<Guid>,ISwitchable, IDateTracking
    {
        public string Value { get; set; }

        public double Price { get; set; }

        public bool? IsGoal { get; set; }


        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }
        public Status Status { set; get; }

        public string TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public SpecialDeType SpecialDeType { get; set; }
        public De_LottoStatus De_LottoStatus { get; set; }
        public RegionStatus RegionStatus { get; set; }

    }
}

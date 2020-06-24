using LotteryManagement.Data.Enums;
using LotteryManagement.Data.Interfaces;
using LotteryManagement.Infrastructure.SharedKernel;
using System;

namespace LotteryManagement.Data.Entities
{
    public class Xien_Lotto : DomainEntity<string>, ISwitchable, IDateTracking
    {


        public string Value { get; set; }

        public Xien_LottoStatus Xien_LottoStatus { get; set; }

        public RegionStatus RegionStatus { get; set; }


        public double Price { get; set; }

        public bool? IsGoal { get; set; }
        /* Tính tỉ lệ */

        public double CurrentRate { get; set; }
        public double BeginRate { get; set; }
        public double EndRate { get; set; }


        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }
        public Status Status { set; get; }


        public string TicketId { get; set; }
        public Ticket Ticket { get; set; }



        public ProvincialCity ProvincialCity { get; set; }
    }

}

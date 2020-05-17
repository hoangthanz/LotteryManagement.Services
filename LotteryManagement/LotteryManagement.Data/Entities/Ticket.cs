using LotteryManagement.Data.Enums;
using LotteryManagement.Data.Interfaces;
using LotteryManagement.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;

namespace LotteryManagement.Data.Entities
{
    public class Ticket : DomainEntity<string>, ISwitchable, IDateTracking
    {
        public string Content { get; set; }
        public double De_Total { get; set; }

        public double Bao_Total { get; set; }
        public double Xien_Total { get; set; }
        public double Cang_Total { get; set; }


        public Status Status { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }


        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }

        public List<Bao_Lotto> Bao_Lottos { get; set; }
        public List<Cang_Lotto> Cang_Lottos { get; set; }
        public List<De_Lotto> De_Lottos { get; set; }
        public List<Xien_Lotto> Xien_Lottos { get; set; }
    }
}

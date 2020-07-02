using LotteryManagement.Data.Enums;
using System;

namespace LotteryManagement.Application.ViewModels.Conditions
{
    public class BettingOnXien
    {
        public string XienArray { get; set; }

        public DivideType DivideType { get; set; }

        public Guid UserId { get; set; }

        public double MultipleNumber { get; set; }

        public Xien_LottoStatus Xien_LottoStatus{ get; set; }

        public RegionStatus RegionStatus { get; set; }

        public ProvincialCity ProvincialCity { get; set; }
    }
}

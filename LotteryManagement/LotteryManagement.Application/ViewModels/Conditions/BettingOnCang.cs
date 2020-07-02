using LotteryManagement.Data.Enums;
using System;

namespace LotteryManagement.Application.ViewModels.Conditions
{
    public class BettingOnCang
    {
        public string CangArray { get; set; }

        public DivideType DivideType { get; set; }

        public Guid UserId { get; set; }

        public double MultipleNumber { get; set; }

        public Cang_LottoStatus Cang_LottoStatus { get; set; }

        public RegionStatus RegionStatus { get; set; }

        public ProvincialCity ProvincialCity { get; set; }
    }
}

using LotteryManagement.Data.Enums;
using System;

namespace LotteryManagement.Application.ViewModels.Conditions
{
    public class BettingOnBaoLo
    {
        public string BaoLoArray { get; set; }

        public DivideType DivideType { get; set; }

        public Guid UserId { get; set; }

        public double MultipleNumber { get; set; }

        public Bao_LottoStatus Bao_LottoStatus { get; set; }

        public RegionStatus RegionStatus { get; set; }

    }
}

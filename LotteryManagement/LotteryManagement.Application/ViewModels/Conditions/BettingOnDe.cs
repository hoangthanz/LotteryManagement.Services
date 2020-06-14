using LotteryManagement.Data.Enums;
using System;

namespace LotteryManagement.Application.ViewModels.Conditions
{
    public class BettingOnDe
    {
        public string DeArray { get; set; }

        public DivideType DivideType { get; set; }

        public Guid UserId { get; set; }

        public double MultipleNumber { get; set; }

        public De_LottoStatus De_LottoStatus { get; set; }

        public RegionStatus RegionStatus { get; set; }

        public SpecialDeType SpecialDeType { get; set; }
    }
}

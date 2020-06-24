using LotteryManagement.Data.Enums;
using LotteryManagement.Data.Interfaces;
using LotteryManagement.Infrastructure.SharedKernel;

namespace LotteryManagement.Data.Entities
{
    public class ProfitPercent : DomainEntity<string>, ISwitchable
    {
        public string Name { get; set; }

        // tỷ lệ sau
        public double Lo2So { get; set; }
        public double Lo2SoDau { get; set; }
        public double Lo2So1K { get; set; }
        public double Lo3So { get; set; }
        public double Lo4So { get; set; }

        public double Xien2 { get; set; }
        public double Xien3 { get; set; }
        public double Xien4 { get; set; }

        public double DeDacBiet { get; set; }
        public double DeDauDacBiet { get; set; }
        public double DeGiai7 { get; set; }
        public double DeGiaiNhat { get; set; }

        public double DeDau { get; set; }
        public double DeDauDuoi { get; set; }

        public double Dau { get; set; }
        public double Duoi{ get; set; }

        public double Cang3 { get; set; }

        public double Cang4 { get; set; }

        public double TruotXien4 { get; set; }
        public double TruotXien8 { get; set; }
        public double TruotXien10 { get; set; }

        public Status Status { get; set; }
        public bool IsUsing { get; set; }

        public RegionStatus RegionStatus { get; set; }

        public ProvincialCity ProvincialCity { get; set; }
    }
}

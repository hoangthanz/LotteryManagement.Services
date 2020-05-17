using LotteryManagement.Data.Enums;
using LotteryManagement.Data.Interfaces;
using LotteryManagement.Infrastructure.SharedKernel;

namespace LotteryManagement.Data.Entities
{
    public class ProfitPercent : DomainEntity<string>, ISwitchable
    {
        public double Lo2SoPercent { get; set; }
        public double Lo2SoDauPercent { get; set; }
        public double Lo2So1KPercent { get; set; }
        public double Lo3SoPercent { get; set; }
        public double Lo4SoPercent { get; set; }

        public double Xien2Percent { get; set; }
        public double Xien3Percent { get; set; }
        public double Xien4Percent { get; set; }

        public double DeDacBietPercent { get; set; }
        public double DeDauDacBietPercent { get; set; }
        public double DeGiai7Percent { get; set; }
        public double DeGiaiNhatPercent { get; set; }

        public double DauPercent { get; set; }
        public double DuoiPercent { get; set; }

        public double Cang3Percent { get; set; }

        public double Cang4Percent { get; set; }

        public double TruotXien4Percent { get; set; }
        public double TruotXien8Percent { get; set; }
        public double TruotXien10Percent { get; set; }


        /* Tỷ lệ số để hiển thị */


        public double Lo2SoPrevious { get; set; }
        public double Lo2SoDauPrevious { get; set; }
        public double Lo2So1KPrevious { get; set; }
        public double Lo3SoPrevious { get; set; }
        public double Lo4SoPrevious { get; set; }

        public double Xien2Previous { get; set; }
        public double Xien3Previous { get; set; }
        public double Xien4Previous { get; set; }

        public double DeDacBietPrevious { get; set; }
        public double DeDauDacBietPrevious { get; set; }
        public double DeGiai7Previous { get; set; }
        public double DeGiaiNhatPrevious { get; set; }

        public double DauPrevious { get; set; }
        public double DuoiPrevious { get; set; }

        public double Cang3Previous { get; set; }

        public double Cang4Previous { get; set; }

        public double TruotXien4Previous { get; set; }
        public double TruotXien8Previous { get; set; }
        public double TruotXien10Previous { get; set; }



        // tỷ lệ sau
        public double Lo2SoAfter { get; set; }
        public double Lo2SoDauAfter { get; set; }
        public double Lo2So1KAfter { get; set; }
        public double Lo3SoAfter { get; set; }
        public double Lo4SoAfter { get; set; }

        public double Xien2After { get; set; }
        public double Xien3After { get; set; }
        public double Xien4After { get; set; }

        public double DeDacBietAfter { get; set; }
        public double DeDauDacBietAfter { get; set; }
        public double DeGiai7After { get; set; }
        public double DeGiaiNhatAfter { get; set; }

        public double DauAfter { get; set; }
        public double DuoiAfter { get; set; }

        public double Cang3After { get; set; }

        public double Cang4After { get; set; }

        public double TruotXien4After { get; set; }
        public double TruotXien8After { get; set; }
        public double TruotXien10After { get; set; }

        public Status Status { get; set; }
    }
}

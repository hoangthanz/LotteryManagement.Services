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



        /*
         * Tỉ lệ lúc ghi - tránh việc sửa ở bảng tỉ lệ
         */
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
        public double Duoi { get; set; }

        public double Cang3 { get; set; }

        public double Cang4 { get; set; }

        public double TruotXien4 { get; set; }
        public double TruotXien8 { get; set; }
        public double TruotXien10 { get; set; }

        public RegionStatus RegionStatus { get; set; }

        public ProvincialCity ProvincialCity { get; set; }

    }
}

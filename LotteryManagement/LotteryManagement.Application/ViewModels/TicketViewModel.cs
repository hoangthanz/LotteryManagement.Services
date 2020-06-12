using LotteryManagement.Data.Entities;
using LotteryManagement.Data.Enums;
using System;
using System.Collections.Generic;

namespace LotteryManagement.Application.ViewModels
{
    public class TicketViewModel
    {
        public string Id { get; set; }

        public string Content { get; set; }
        public double De_Total { get; set; }

        public double Bao_Total { get; set; }
        public double Xien_Total { get; set; }
        public double Cang_Total { get; set; }


        public Status Status { set; get; }
        public DateTime DateCreated { set; get; }
        public DateTime? DateModified { set; get; }



        public Guid UserId { get; set; }
        public AppUserViewModel AppUser { get; set; }

        public List<Bao_Lotto> Bao_Lottos { get; set; }
        public List<Cang_Lotto> Cang_Lottos { get; set; }
        public List<De_Lotto> De_Lottos { get; set; }
        public List<Xien_Lotto> Xien_Lottos { get; set; }



        /*
         * Tỉ lệ lúc ghi - tránh việc sửa ở bảng tỉ lệ
         */
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
    }
}

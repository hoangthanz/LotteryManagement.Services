using System.ComponentModel;

namespace LotteryManagement.Data.Enums
{
    public enum TransactionHistoryStatus
    {
        [Description("Nạp tiền")]
        PayIn,
        [Description("Đặt cược")]
        ToBet,
        [Description("Trao thưởng")]
        ToReward,
        [Description("Rút tiền")]
        Withdraw,


    }
}

using System.ComponentModel;

namespace LotteryManagement.Data.Enums
{
    public enum TransactionHistoryType
    {
        [Description("Nạp tiền")]
        PayIn,
        [Description("Đặt cược")]
        ToBet,
        [Description("Trao thưởng")]
        ToReward,
        [Description("Rút tiền")]
        Withdraw,
        [Description("Rút tiền và nạp")]
        PayInAndWithdraw,
        [Description("Đặt cược và trao thưởng")]
        ToBetAndToReward,
    }
}

using System.ComponentModel;

namespace LotteryManagement.Data.Enums
{

    public enum TransactionType
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
    }
}

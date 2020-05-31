using System.ComponentModel;

namespace LotteryManagement.Data.Enums
{
    public enum BillStatus
    {
        [Description("Đang xử lý")]
        InProgress,
        [Description("Hủy")]
        Cancelled,
        [Description("Hoàn thành")]
        Completed,
        [Description("Hoàn thành và hủy")]
        CompletedAndCancelled
    }
}

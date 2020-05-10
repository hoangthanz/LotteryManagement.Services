using LotteryManagement.Data.Enums;

namespace LotteryManagement.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}

using System.ComponentModel;

namespace LotteryManagement.Application.ViewModels
{
    public enum  DivideType
    {
        [Description(",")]
        Comma,
        [Description(";")]
        SemiColon,
        [Description(" ")]
        Space
    }
}

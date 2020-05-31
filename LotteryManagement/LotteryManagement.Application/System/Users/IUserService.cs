
using LotteryManagement.Application.ViewModels;
using LotteryManagement.Application.ViewModels.Users;
using System.Threading.Tasks;

namespace LotteryManagement.Application.System.Users
{
    public interface IUserService
    {

        Task<string> Authencate(LoginRequest request);

        Task<string> AuthencateForClient(LoginRequest request);

        Task<bool> Register(AppUserViewModel request);

    }
}

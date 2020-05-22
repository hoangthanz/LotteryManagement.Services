using LotteryManagement.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LotteryManagement.Application.Interfaces
{
    public interface IAppUserService
    {
        Task<AppUserViewModel> Add(AppUserViewModel appUser, string password);

        void Update(AppUserViewModel appUser);

        void Delete(string id);

        void SaveChanges();

        Task<List<AppUserViewModel>> GetAppUsers();

        Task<AppUserViewModel> GetAppUsersById(string id);
    }
}

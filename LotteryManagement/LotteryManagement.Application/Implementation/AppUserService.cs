using AutoMapper;
using LotteryManagement.Application.Interfaces;
using LotteryManagement.Application.ViewModels;
using LotteryManagement.Data.EF;
using LotteryManagement.Data.Entities;
using LotteryManagement.Infrastructure.Interfaces;
using LotteryManagement.Utilities.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LotteryManagement.Application.Implementation
{
    public class AppUserService : IAppUserService
    {

        private IRepository<AppUser, Guid> _appUserRepository;
        private IUnitOfWork _unitOfWork;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<AppRole> _roleManager;
        private LotteryManageDbContext _context;

        public AppUserService(
            IRepository<AppUser, Guid> appUserRepository,
            IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            LotteryManageDbContext context
            )
        {
            _appUserRepository = appUserRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }



        public async void Delete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                _context.AppUsers.Remove(user);
                
            }
            catch (LotteryManagementException)
            {

                throw new LotteryManagementException("Lỗi hệ thống!");
            }
            catch (Exception)
            {

                throw new Exception();
            }
            
            
        }

        public async Task<AppUserViewModel> GetAppUsersById(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                var result = Mapper.Map<AppUser, AppUserViewModel>(user);
                return result;

            }
            catch (LotteryManagementException)
            {

                throw new LotteryManagementException("Lỗi hệ thống!");
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }

        public async Task<List<AppUserViewModel>> GetAppUsers()
        {
            try
            {
                var users = await _context.AppUsers.ToListAsync();
                var userViews = Mapper.Map<List<AppUser>, List<AppUserViewModel>>(users);
                return userViews;

            }
            catch (LotteryManagementException)
            {

                throw new LotteryManagementException("Lỗi hệ thống!");
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public async void Update(AppUserViewModel appUserView)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(appUserView.Id.ToString());
                user.FirstName = appUserView.FirstName;
                user.LastName = appUserView.LastName;
                user.NickName = appUserView.NickName;
                user.DateOfBirth = appUserView.DateOfBirth;

                _context.AppUsers.Update(user);

            }
            catch (LotteryManagementException)
            {

                throw new LotteryManagementException("Lỗi hệ thống!");
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }

        public async Task<AppUserViewModel> Add(AppUserViewModel appUserView, string password)
        {
            try
            {
                var appUser = Mapper.Map<AppUserViewModel, AppUser>(appUserView);

                var result = await _userManager.CreateAsync(appUser, password);
                if (result.Succeeded)
                {
                    return appUserView;
                }
                return new AppUserViewModel();
            }
            catch (LotteryManagementException)
            {

                throw new LotteryManagementException("Lỗi hệ thống!");
            }
            catch (Exception)
            {

                throw new Exception();
            }

        }


    }
}

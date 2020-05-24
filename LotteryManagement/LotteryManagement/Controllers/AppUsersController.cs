﻿using AutoMapper;
using LotteryManagement.Application.ViewModels;
using LotteryManagement.Data.EF;
using LotteryManagement.Data.Entities;
using LotteryManagement.Data.Enums;
using LotteryManagement.Utilities.Dtos;
using LotteryManagement.Utilities.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LotteryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        private readonly LotteryManageDbContext _context;
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private RoleManager<AppRole> _roleManager;

        public AppUsersController(
            LotteryManageDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager
        )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }

        // GET: api/AppUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserViewModel>>> GetAppUsers()
        {

            var appUsers = _context.AppUsers.ToList();
            var result = Mapper.Map<List<AppUser>, List<AppUserViewModel>>(appUsers);
            return result;
        }

        // GET: api/AppUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetAppUser(Guid id)
        {
            var appUser = await _context.AppUsers.FindAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            return appUser;
        }

        // PUT: api/AppUsers/5

        [HttpPut("{id}")]
        public async Task<ActionResult<Object>> PutAppUser(string id, AppUserViewModel appUser)
        {
            if (id != appUser.Id.ToString())
            {
                return BadRequest();
            }




            try
            {
                AppUser currentUser = await _userManager.FindByIdAsync(id);
                if (currentUser == null)
                    return NotFound(new ResponseResult("Không tìm thấy tài khoản này"));

                currentUser.FirstName = appUser.FirstName;
                currentUser.LastName = appUser.LastName;
                currentUser.NickName = appUser.NickName;
                currentUser.PhoneNumber = appUser.PhoneNumber;
                currentUser.DateOfBirth = appUser.DateOfBirth;
                currentUser.Avatar = appUser.Avatar;

                IdentityResult result = await _userManager.UpdateAsync(currentUser);
                if (result.Succeeded)
                    return currentUser;
                else
                {
                    string error = "";
                    foreach (IdentityError e in result.Errors)
                        error += e;
                    return BadRequest(new ResponseResult(error));
                }


            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppUserExists(Guid.Parse(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


        }

        // POST: api/AppUsers
        [HttpPost]
        public async Task<ActionResult<Object>> PostAppUser(AppUserViewModel appUserView)
        {
            try
            {
                if (EmailExits(appUserView.Email) || string.IsNullOrEmpty(appUserView.Email))
                {
                    return BadRequest(new ResponseResult("Email đã tồn tại hoặc bị bỏ trống!"));
                }

                var user = new AppUser()
                {
                    DateOfBirth = appUserView.DateOfBirth,
                    Email = appUserView.Email,
                    FirstName = appUserView.FirstName,
                    LastName = appUserView.LastName,
                    UserName = appUserView.UserName,
                    PhoneNumber = appUserView.PhoneNumber,
                    Avatar = appUserView.Avatar,
                    NickName = appUserView.NickName,
                    TransactionPassword = appUserView.TransactionPassword,
                    Id = Guid.NewGuid()

                };

                if (!string.IsNullOrEmpty(appUserView.RootUserId))
                {
                    // generate link ref
                    var rootUser = _context.AppUsers.Where(x => x.Id.ToString() == appUserView.RootUserId).FirstOrDefault();
                    if (rootUser != null)
                    {
                        user.RootUserId = rootUser.Id;
                        user.RefRegisterLink = "/api/Users/register/" + rootUser.Id.ToString();
                    }
                }


                // create wallet
                var wallet = new Wallet
                {
                    DateCreated = DateTime.Now,
                    Coin = 0,
                    PendingCoin = 0,
                    PromotionCoin = 0,
                    Status = Status.Active,
                    Id = TextHelper.RandomString(10),
                    UserId = ""
                };

                _context.Wallets.Add(wallet);

                await _context.SaveChangesAsync();
                var newWallet = _context.Wallets.Where(x => string.IsNullOrEmpty(x.UserId)).FirstOrDefault();

                user.WalletId = newWallet.Id;


                var result = await _userManager.CreateAsync(user, appUserView.Password);

                if (result.Succeeded)
                {

                    newWallet.UserId = user.Id.ToString();

                    await _context.SaveChangesAsync();

                    var createdUser = await _context.AppUsers.Where(x => x.Id == user.Id).FirstOrDefaultAsync();

                    var createdUserView = Mapper.Map<AppUser, AppUserViewModel>(createdUser);
                    return createdUserView;
;
                }
                else
                {
                    _context.Wallets.Remove(newWallet);
                    await _context.SaveChangesAsync();
                    string error = "";
                    foreach (var e in result.Errors)
                        error += e.Description;
                    return BadRequest(new ResponseResult(error));
                }

            }
            catch (DbUpdateConcurrencyException)
            {

                throw new DbUpdateConcurrencyException("Lỗi hệ thống!");
            }
            catch (Exception e)
            {

                throw new Exception();
            }

        }

        // DELETE: api/AppUsers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseResult>> DeleteAppUser(string id)
        {
            var appUser = await _context.AppUsers.FindAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            _context.AppUsers.Remove(appUser);
            await _context.SaveChangesAsync();

            return new ResponseResult("Xóa tài khoản thành công!");
        }

        private bool AppUserExists(Guid id)
        {
            return _context.AppUsers.Any(e => e.Id == id);
        }

        private bool EmailExits(string email)
        {
            return _context.AppUsers.Any(x => x.Email == email);
        }
    }
}
using AutoMapper;
using LotteryManagement.Application.ViewModels;
using LotteryManagement.Application.ViewModels.Users;
using LotteryManagement.Data.EF;
using LotteryManagement.Data.Entities;
using LotteryManagement.Data.Enums;
using LotteryManagement.Utilities.Constants;
using LotteryManagement.Utilities.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LotteryManagement.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly LotteryManageDbContext _context;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IConfiguration config,
            LotteryManageDbContext context,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _context = context;

            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> Authencate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return null;

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return null;
            }


            var per = (from u in _context.AppUsers
                       join p in _context.Permissions on u.Id equals p.UserId
                       join f in _context.Functions on p.FunctionId equals f.Id
                       where user.Id == u.Id
                       select new
                       {
                           f.Name,
                           f.Code,
                       }).ToList();


            var claims = new[]
            {
                new Claim("Email",user.Email),
                new Claim("Name",(!string.IsNullOrEmpty(user.FirstName) || !string.IsNullOrEmpty(user.LastName))?user.FirstName + " " + user.LastName : ""),
                new Claim("PhoneNumber", user.PhoneNumber),
                new Claim("UserName", user.UserName),
                new Claim("Id", user.Id.ToString()),
                new Claim("Permission", per[0].Code),
                new Claim("RefLink", user.RefRegisterLink),
                new Claim("TestPermission", per.ToList().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<string> AuthencateForClient(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return null;

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return null;
            }

            var wallet = _context.Wallets.Where(x => x.WalletId == user.WalletId).FirstOrDefault();
      

            var claims = new[]
            {
                new Claim("Email",user.Email),
                new Claim("Name",(!string.IsNullOrEmpty(user.FirstName) || !string.IsNullOrEmpty(user.LastName))?user.FirstName + " " + user.LastName : ""),
                new Claim("PhoneNumber", user.PhoneNumber),
                new Claim("UserName", user.UserName),
                new Claim("Id", user.Id.ToString()),
                new Claim("Wallet", JsonConvert.SerializeObject(wallet)),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Đăng ký thường không qua tài khoản nào

        public async Task<bool> Register(AppUserViewModel request)
        {
            var user = new AppUser()
            {
                DateOfBirth = request.DateOfBirth,
                Email = request.Email,
                FirstName = (string.IsNullOrEmpty(request.FirstName))? request.FirstName: "" ,
                LastName = (string.IsNullOrEmpty(request.LastName)) ? request.LastName : "",
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Avatar = request.Avatar,
                NickName = request.NickName,
                TransactionPassword = request.TransactionPassword,
                Id = Guid.NewGuid()
            };

            if (!string.IsNullOrEmpty(request.RootUserId))
            {
                // generate link ref
                var rootUser = _context.AppUsers.Where(x => x.Id.ToString() == request.RootUserId).FirstOrDefault();
                if (rootUser != null)
                {
                    user.RootUserId = rootUser.Id;
                    user.RefRegisterLink = DomainConsts.LotteryDomain + "/api/Users/register/" + user.Id.ToString();
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
                WalletId = TextHelper.RandomString(10),
                Id = Guid.NewGuid().ToString(),
                UserId = ""
            };

            _context.Wallets.Add(wallet);

            await _context.SaveChangesAsync();

            var newWallet = _context.Wallets.Where(x => string.IsNullOrEmpty(x.UserId)).FirstOrDefault();

            user.WalletId = newWallet.WalletId;

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {

                newWallet.UserId = user.Id.ToString();

                await _context.SaveChangesAsync();

                var createdUser = await _context.AppUsers.Where(x => x.Id == user.Id).FirstOrDefaultAsync();

                var createdUserView = Mapper.Map<AppUser, AppUserViewModel>(createdUser);
                return true;
            }
            else
            {
                _context.Wallets.Remove(newWallet);
                await _context.SaveChangesAsync();
                string error = "";
                foreach (var e in result.Errors)
                    error += e.Description;
                return false;
            }

        }

    }
}

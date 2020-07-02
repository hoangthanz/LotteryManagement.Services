using LotteryManagement.Data.EF;
using LotteryManagement.Data.Entities;
using LotteryManagement.Data.Enums;
using LotteryManagement.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LotteryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitializeController : ControllerBase
    {
        private readonly LotteryManageDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public InitializeController(
            LotteryManageDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("Initialize-Database")]
        [AllowAnonymous]
        public async Task<IActionResult> SeedData()
        {
            try
            {
                if (_context.Functions.ToList().Count == 0)
                {
                    List<Function> functions = new List<Function>()
                    {
                        new Function(){ Id = TextHelper.RandomString(10, false), Name = "Quyền Admin", Code = "ExportXLS", Status = Status.Active},
                        new Function(){ Id = TextHelper.RandomString(10, false), Name = "Điều chỉnh tài khoản", Code = "Admin", Status = Status.Active},
                        new Function(){ Id = TextHelper.RandomString(10, false), Name = "", Code = "Player", Status = Status.Active},

                        new Function(){ Id = TextHelper.RandomString(10, false), Name = "", Code = "ProfitPercent.Read", Status = Status.Active},
                        new Function(){ Id = TextHelper.RandomString(10, false), Name = "", Code = "ProfitPercent.Write", Status = Status.Active},
                        new Function(){ Id = TextHelper.RandomString(10, false), Name = "", Code = "ProfitPercent.Delete", Status = Status.Active},

                        new Function(){ Id = TextHelper.RandomString(10, false), Name = "", Code = "Account.Read", Status = Status.Active},
                        new Function(){ Id = TextHelper.RandomString(10, false), Name = "", Code = "Account.Write", Status = Status.Active},
                        new Function(){ Id = TextHelper.RandomString(10, false), Name = "", Code = "Account.Delete", Status = Status.Active},

                        new Function(){ Id = TextHelper.RandomString(10, false), Name = "", Code = "Transaction.Read", Status = Status.Active},
                        new Function(){ Id = TextHelper.RandomString(10, false), Name = "", Code = "Transaction.Write", Status = Status.Active},
                        new Function(){ Id = TextHelper.RandomString(10, false), Name = "", Code = "Transaction.Delete", Status = Status.Active},

                    };

                    await _context.Functions.AddRangeAsync(functions);
                    await _context.SaveChangesAsync();

                }


                if (_context.AppUsers.ToList().Count == 0)
                {

                    var user = new AppUser()
                    {
                        DateOfBirth = DateTime.Now,
                        Email = "admin@gmail.com",
                        FirstName = "Admin",
                        LastName = "Lottery",
                        UserName = "admin",
                        PhoneNumber = "0900000000",
                        Avatar = "",
                        NickName = "Adminstrator",
                        TransactionPassword = "123456",
                        RefRegisterLink = "",
                        WalletId = ""
                    };
                    var result = await _userManager.CreateAsync(user, "123123aA@");
                    await _context.AppUsers.AddAsync(user);
                }

                if (!_context.Wallets.Any())
                {
                    var user = _context.AppUsers.Where(x => x.UserName == "admin").FirstOrDefault();

                    var wallet = new Wallet
                    {
                        DateCreated = DateTime.Now,
                        PendingCoin = 0,
                        Coin = 999999999,
                        Id = Guid.NewGuid().ToString(),
                        Status = Status.Active,
                        PromotionCoin = 0,
                        WalletId = TextHelper.RandomString(10),
                        UserId = user.Id.ToString()
                    };

                    string walletId = wallet.WalletId;

                    await _context.Wallets.AddAsync(wallet);
                    _context.SaveChanges();

                    user.WalletId = walletId;
                    _context.AppUsers.Update(user);
                    _context.SaveChanges();

                }


                if (!_context.OwnerBanks.Any())
                {
                    var banks = new List<OwnerBank>
                    {
                        new OwnerBank{
                            FullNameOwner = "Adminstrator",
                            BankName = "Ngân hàng ABC",
                            AccountNumber = "xxxx xxxx xxxx xxxx",
                            Branch =  "Chi nhánh VN",
                        },
                        new OwnerBank{
                            FullNameOwner = "Adminstrator",
                            BankName = "Ngân hàng ABC 1",
                            AccountNumber = "xxxx xxxx xxxx xxxx",
                            Branch =  "Chi nhánh VN",
                        },
                        new OwnerBank{
                            FullNameOwner = "Adminstrator",
                            BankName = "Ngân hàng ABC 2",
                            AccountNumber = "xxxx xxxx xxxx xxxx",
                            Branch =  "Chi nhánh VN",
                        }
                    };

                    _context.OwnerBanks.AddRange(banks);
                    _context.SaveChanges();
                }

                if (_context.Permissions.ToList().Count == 0)
                {
                    List<Permission> permissions = new List<Permission>();

                    var user = _context.Users.ToList()[0];
                    foreach (var fun in _context.Functions.ToList())
                    {
                        var per = new Permission() { Id = Guid.NewGuid().ToString(), DateCreated = DateTime.Now, Status = Status.Active, UserId = user.Id, FunctionId = fun.Id };
                        permissions.Add(per);
                    }

                    await _context.Permissions.AddRangeAsync(permissions);
                }
                await _context.SaveChangesAsync();
                return Ok("Đã tạo được dữ liệu mẫu thành công");
            }
            catch
            {
                return BadRequest("Không tạo được dữ liệu mẫu");
            }

        }
    }
}
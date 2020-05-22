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
    public class TestController : ControllerBase
    {

        private readonly LotteryManageDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public TestController(
            LotteryManageDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("SeedData")]
        [AllowAnonymous]
        public async Task<IActionResult> SeedData()
        {
            try
            {
                if (_context.Functions.ToList().Count == 0)
                {
                    List<Function> functions = new List<Function>()
                    {
                        new Function(){ Id = TextHelper.RandomString(10, false), Name = "Điều chỉnh tài khoản", Code = "Admin", Status = Status.Active},
                    };

                    //var fun = new Function() { Id = TextHelper.RandomString(10, false), Name = "Điều chỉnh tài khoản", Code = "Account", Status = Status.Active };
                    await _context.Functions.AddRangeAsync(functions);

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
                        PhoneNumber = "0953421059",
                        Avatar = "",
                        NickName = "admin",
                        TransactionPassword = "123456",
                        RefRegisterLink = "x",
                        
                        WalletId = "QAZWSXEDC"
                    };
                    var result = await _userManager.CreateAsync(user, "123123aA@");
                    await _context.AppUsers.AddAsync(user);
                }


                if (_context.Permissions.ToList().Count == 0)
                {
                    List<Permission> permissions = new List<Permission>()
                    {
                        new Permission()
                        {
                            Id = TextHelper.RandomString(30, false),
                            DateCreated = DateTime.Now,
                            FunctionId = _context.Functions.ToList()[0].Id,
                            UserId = _context.AppUsers.ToList()[0].Id,
                            hasCreate = true,
                            hasUpdate = true,
                            hasRead = true,
                            hasDelete = true,
                            Status = Status.Active
                        }
                    };

                    

                   
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
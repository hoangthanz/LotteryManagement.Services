using LotteryManagement.Data.EF;
using LotteryManagement.Data.Entities;
using LotteryManagement.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            if (_context.Functions.ToList().Count == 0)
            {
                List<Function> functions = new List<Function>()
                {
                    new Function(){ Name = "Điều chỉnh tỷ lệ", Code = "", Status = Status.Active},
                    new Function(){ Name = "", Code = "", Status = Status.Active},
                };
            }


            return Ok();
        }
    }
}
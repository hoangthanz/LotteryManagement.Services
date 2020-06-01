using LotteryManagement.Application.System.Users;
using LotteryManagement.Application.ViewModels;
using LotteryManagement.Application.ViewModels.Users;
using LotteryManagement.Utilities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LotteryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultToken = await _userService.Authencate(request);
            if (string.IsNullOrEmpty(resultToken))
            {
                return BadRequest(new ResponseResult("Tên đăng nhập hoặc mật khẩu không đúng!"));
            }
            return Ok(new { token = resultToken });
        }


        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(AppUserViewModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Register(request);
            if (!result)
            {
                return BadRequest(new ResponseResult("Đăng ký không thành công!, yêu cầu kiểm tra lại mật khẩu và điền đầy đủ!"));
            }
            return Ok();
        }

        [HttpPost("register/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromForm]AppUserViewModel request,string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.RootUserId = id;

            var result = await _userService.Register(request);
            if (!result)
            {
                return BadRequest("Register is unsuccessful.");
            }
            return Ok();
        }


    }
}
using LotteryManagement.Data.EF;
using LotteryManagement.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LotteryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Xien_LottoController : ControllerBase
    {
        private readonly LotteryManageDbContext _context;

        public Xien_LottoController(LotteryManageDbContext context)
        {
            _context = context;
        }

        private bool IsNumber(string str) => int.TryParse(str, out int n);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Xien_Lotto>>> GetXien_Lottos()
        {
            return await _context.Xien_Lottos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Xien_Lotto>> GetXien_Lotto(string id)
        {
            var xien_Lotto = await _context.Xien_Lottos.FindAsync(id);

            if (xien_Lotto == null)
            {
                return NotFound();
            }

            return xien_Lotto;
        }

  
    }
}

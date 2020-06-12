using LotteryManagement.Application.ViewModels.Conditions;
using LotteryManagement.Data.EF;
using LotteryManagement.Data.Entities;
using LotteryManagement.Utilities.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LotteryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class De_LottoController : ControllerBase
    {
        private readonly LotteryManageDbContext _context;

        public De_LottoController(LotteryManageDbContext context)
        {
            _context = context;
        }

        // GET: api/De_Lotto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<De_Lotto>>> GetDe_Lottos()
        {
            return await _context.De_Lottos.ToListAsync();
        }

        // GET: api/De_Lotto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<De_Lotto>> GetDe_Lotto(string id)
        {
            var de_Lotto = await _context.De_Lottos.FindAsync(id);

            if (de_Lotto == null)
            {
                return NotFound();
            }

            return de_Lotto;
        }

        // PUT: api/De_Lotto/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDe_Lotto(string id, De_Lotto de_Lotto)
        {
            if (id != de_Lotto.Id)
            {
                return BadRequest();
            }

            _context.Entry(de_Lotto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!De_LottoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/De_Lotto
        [HttpPost]
        public async Task<ActionResult<De_Lotto>> PostDe_Lotto(De_Lotto de_Lotto)
        {
            _context.De_Lottos.Add(de_Lotto);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (De_LottoExists(de_Lotto.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDe_Lotto", new { id = de_Lotto.Id }, de_Lotto);
        }


        [HttpPost("betting-on")]
        public async Task<ActionResult<object>> Post_BettingOn_De_Lotto(BettingOnDe bettingOnDe)
        {

            /*
             * Nhận về là 1 string => danh sách các con số được ngăn cách nhau bởi dấu ";" or "," or " " 
             * Xử lí: Tách chuỗi để lấy ra các con đề rồi insert vào db với các tài khoản tương ứng ticket
             */
            try
            {
                if (string.IsNullOrEmpty(bettingOnDe.DeArray))
                {
                    return BadRequest(new ResponseResult("Lỗi đặt cược!, danh sách cược không thể để trống."));
                }

                string[] deArrayByComma = bettingOnDe.DeArray.Split(',');
                string[] deArrayBySemiColon = bettingOnDe.DeArray.Split(';');
                string[] deArrayBySpace = bettingOnDe.DeArray.Split(' ');

                

                string x = "";
            }
            catch (DbUpdateException)
            {

                throw;

            }

            return "";
        }

        // DELETE: api/De_Lotto/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<De_Lotto>> DeleteDe_Lotto(string id)
        {
            var de_Lotto = await _context.De_Lottos.FindAsync(id);
            if (de_Lotto == null)
            {
                return NotFound();
            }

            _context.De_Lottos.Remove(de_Lotto);
            await _context.SaveChangesAsync();

            return de_Lotto;
        }

        private bool De_LottoExists(string id)
        {
            return _context.De_Lottos.Any(e => e.Id == id);
        }



        private bool DeFomatCheck(List<string> des)
        {
            foreach (var item in des)
            {
                if(item.Length != 2)
                {
                    return false;
                }
            }
            return true;
        }
        
    }
}

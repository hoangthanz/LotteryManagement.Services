using LotteryManagement.Application.Interfaces;
using LotteryManagement.Application.ViewModels;
using LotteryManagement.Data.EF;
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
    public class ProfitPercentsController : ControllerBase
    {
        private readonly IProfitPercentService _profitPercentService;
        private readonly LotteryManageDbContext _context;
        public ProfitPercentsController(IProfitPercentService profitPercentService,
            LotteryManageDbContext context)
        {
            _profitPercentService = profitPercentService;
            _context = context;
        }

        // GET: api/ProfitPercents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfitPercentViewModel>>> GetProfitPercents()
        {
            return await _profitPercentService.GetProfitPercents();
        }

        // GET: api/ProfitPercents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfitPercentViewModel>> GetProfitPercent(string id)
        {
            var profitPercent = await _profitPercentService.GetProfitPercentById(id);

            if (profitPercent == null)
            {
                return NotFound();
            }

            return profitPercent;
        }

        // PUT: api/ProfitPercents/5
        [HttpPut("{id}")]
        public IActionResult PutProfitPercent(string id, ProfitPercentViewModel profitPercent)
        {
            if (id != profitPercent.Id)
            {
                return BadRequest();
            }


            try
            {
                _profitPercentService.Update(profitPercent);
                _profitPercentService.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfitPercentExists(id))
                {
                    new GenericResult(false, "Đã tồn tại tỷ lệ cược này!");
                }
                else
                {
                    new GenericResult(false, "Lỗi hệ thống!");
                }
            }

            return NoContent();
        }

        // POST: api/ProfitPercents

        [HttpPost]
        public ActionResult<ProfitPercentViewModel> PostProfitPercent(ProfitPercentViewModel profitPercent)
        {
            _profitPercentService.Add(profitPercent);

            try
            {
                _profitPercentService.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ProfitPercentExists(profitPercent.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProfitPercent", new { id = profitPercent.Id }, profitPercent);
        }

        // DELETE: api/ProfitPercents/5
        [HttpDelete("{id}")]
        public ActionResult<GenericResult> DeleteProfitPercent(string id)
        {
            var profitPercent = _profitPercentService.GetProfitPercentById(id);
            if (profitPercent == null)
            {
                return new GenericResult(false, "Không tồn tại tỷ lệ cược này");
            }

            _profitPercentService.Delete(id);
            _profitPercentService.SaveChanges();



            return new GenericResult(true,"Xóa thánh công tỷ lệ cược");
            
        }

        private bool ProfitPercentExists(string id)
        {
            return _context.ProfitPercents.Any(e => e.Id == id);
        }
    }
}

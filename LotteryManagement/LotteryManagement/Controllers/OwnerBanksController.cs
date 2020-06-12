using LotteryManagement.Data.EF;
using LotteryManagement.Data.Entities;
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
    public class OwnerBanksController : ControllerBase
    {
        private readonly LotteryManageDbContext _context;

        public OwnerBanksController(LotteryManageDbContext context)
        {
            _context = context;
        }

        // GET: api/OwnerBanks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OwnerBank>>> GetOwnerBanks()
        {
            return await _context.OwnerBanks.ToListAsync();
        }

        // GET: api/OwnerBanks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerBank>> GetOwnerBank(Guid id)
        {
            var ownerBank = await _context.OwnerBanks.FindAsync(id);

            if (ownerBank == null)
            {
                return NotFound();
            }

            return ownerBank;
        }

        // PUT: api/OwnerBanks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOwnerBank(Guid id, OwnerBank ownerBank)
        {
            if (id != ownerBank.Id)
            {
                return BadRequest();
            }

            _context.Entry(ownerBank).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnerBankExists(id))
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

        // POST: api/OwnerBanks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<OwnerBank>> PostOwnerBank(OwnerBank ownerBank)
        {
            _context.OwnerBanks.Add(ownerBank);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOwnerBank", new { id = ownerBank.Id }, ownerBank);
        }

        // DELETE: api/OwnerBanks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OwnerBank>> DeleteOwnerBank(Guid id)
        {
            var ownerBank = await _context.OwnerBanks.FindAsync(id);
            if (ownerBank == null)
            {
                return NotFound();
            }

            _context.OwnerBanks.Remove(ownerBank);
            await _context.SaveChangesAsync();

            return ownerBank;
        }

        private bool OwnerBankExists(Guid id)
        {
            return _context.OwnerBanks.Any(e => e.Id == id);
        }
    }
}

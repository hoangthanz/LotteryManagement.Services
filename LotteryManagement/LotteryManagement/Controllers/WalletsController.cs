using AutoMapper;
using LotteryManagement.Application.ViewModels;
using LotteryManagement.Data.EF;
using LotteryManagement.Data.Entities;
using LotteryManagement.Data.Enums;
using LotteryManagement.Utilities.Dtos;
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
    public class WalletsController : ControllerBase
    {
        private readonly LotteryManageDbContext _context;

        public WalletsController(LotteryManageDbContext context)
        {
            _context = context;
        }

        // GET: api/Wallets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WalletViewModel>>> GetWallets()
        {
            var wallets = await _context.Wallets.Where(x => !string.IsNullOrEmpty(x.UserId)).ToListAsync();
            var walletViews = Mapper.Map<List<Wallet>, List<WalletViewModel>>(wallets);

            foreach (var wallet in walletViews)
            {
                foreach (var user in _context.AppUsers)
                {
                    if (user.WalletId == wallet.WalletId)
                    {
                        wallet.AppUser = Mapper.Map<AppUser, AppUserViewModel>(user);
                    }
                }
            }

           
            return walletViews;
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<Wallet>> GetWalletByUser(string id)
        {
            var wallet = await _context.Wallets.Where(x => x.UserId == id).FirstOrDefaultAsync();

            if (wallet == null)
            {
                return BadRequest(new ResponseResult("Không tìm thấy ví!"));
            }

            return wallet;
        }

        // GET: api/Wallets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wallet>> GetWallet(string id)
        {
            var wallet = await _context.Wallets.FindAsync(id);

            if (wallet == null)
            {
                return BadRequest(new ResponseResult("Không tìm thấy ví!"));
            }

            return wallet;
        }

        // PUT: api/Wallets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWallet(string id, WalletViewModel wallet)
        {
            if (id != wallet.Id)
            {
                return BadRequest();
            }

            try
            {
                var currentWallet = await _context.Wallets.Where(x => x.Id == id).FirstOrDefaultAsync();
                var user = await _context.AppUsers.Where(x => x.Id.ToString() == currentWallet.UserId).FirstOrDefaultAsync();
                var newCoin = wallet.Coin - currentWallet.Coin;
                var newPromotionCoin = wallet.PromotionCoin - currentWallet.PendingCoin;
                
                var transactionHistory = new TransactionHistory
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = wallet.UserId,
                    BillStatus = BillStatus.Completed,
                    Coin = Math.Abs(newCoin),
                    DateCreated = DateTime.Now,
                    Status = Status.Active,
                    TransactionHistoryType = (newCoin > 0) ? TransactionHistoryType.PayIn : TransactionHistoryType.Withdraw,
                };

                string notifyTransaction = " đã nạp ";
                if (transactionHistory.TransactionHistoryType == TransactionHistoryType.Withdraw)
                    notifyTransaction = " đã rút ";

                transactionHistory.Content = "Tài khoản " + user.UserName + notifyTransaction + newCoin + " vào lúc "  + transactionHistory.DateCreated.ToString("dd/MM/yyyy hh:mm:ss tt");

                _context.TransactionHistories.Add(transactionHistory);


                currentWallet.PromotionCoin = wallet.PromotionCoin;
                currentWallet.Coin = wallet.Coin;
                _context.Wallets.Update(currentWallet);


                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WalletExists(id))
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

        // POST: api/Wallets

        [HttpPost]
        public async Task<ActionResult<Wallet>> PostWallet(Wallet wallet)
        {
            _context.Wallets.Add(wallet);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WalletExists(wallet.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWallet", new { id = wallet.Id }, wallet);
        }

        // DELETE: api/Wallets/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Wallet>> DeleteWallet(string id)
        {
            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }

            _context.Wallets.Remove(wallet);
            await _context.SaveChangesAsync();

            return wallet;
        }

        private bool WalletExists(string id)
        {
            return _context.Wallets.Any(e => e.Id == id);
        }
    }
}

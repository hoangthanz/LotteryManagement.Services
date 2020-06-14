using AutoMapper;
using LotteryManagement.Application.ViewModels;
using LotteryManagement.Application.ViewModels.Conditions;
using LotteryManagement.Data.EF;
using LotteryManagement.Data.Entities;
using LotteryManagement.Data.Enums;
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
    public class TransactionHistoriesController : ControllerBase
    {
        private readonly LotteryManageDbContext _context;

        public TransactionHistoriesController(LotteryManageDbContext context)
        {
            _context = context;
        }

        // GET: api/TransactionHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionHistory>>> GetTransactionHistories()
        {
            return await _context.TransactionHistories.ToListAsync();
        }

        // GET: api/TransactionHistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionHistory>> GetTransactionHistory(string id)
        {
            var transactionHistory = await _context.TransactionHistories.FindAsync(id);

            if (transactionHistory == null)
            {
                return NotFound();
            }

            return transactionHistory;
        }

        [HttpPost("/condition")]
        public async Task<ActionResult<IEnumerable<TransactionHistoryViewModel>>> GetTransactionHistoriesByCondition(TransactionHistoryCondition condition)
        {
            try
            {
                var transactionHistory = _context.TransactionHistories.Where(x => x.Status == Status.Active).OrderByDescending(x => x.DateCreated).ToList();

                if(condition.FromDate != null)
                {
                    transactionHistory = transactionHistory.Where(x => x.DateCreated >= condition.FromDate && x.DateCreated <= condition.ToDate).ToList();
                }

                var transactionHistoryViewModel = Mapper.Map<List<TransactionHistory>, List<TransactionHistoryViewModel>>(transactionHistory);

                foreach (var item in transactionHistoryViewModel)
                {
                    var user = await _context.AppUsers.Where(x => x.Id.ToString() == item.UserId).FirstOrDefaultAsync();
                    var banckCard = await _context.BankCards.Where(x => x.Id == item.BankCardId).FirstOrDefaultAsync();
                    var ownerBank = await _context.OwnerBanks.Where(x => x.Id.ToString() == item.OwnerBankId).FirstOrDefaultAsync();

                    if(user != null)
                    {
                        var userView = Mapper.Map<AppUser, AppUserViewModel>(user);
                        item.AppUser = userView;
                    } 

                    if(banckCard != null)
                    {
                        var bankCardView = Mapper.Map<BankCard, BankCardViewModel>(banckCard);
                        item.BankCardId = banckCard.Id;
                        item.BankCardViewModel = bankCardView;
                    }
                    
                    if(ownerBank != null)
                    {
                        item.OwnerBankId = ownerBank.Id.ToString();
                        item.OwnerBankViewModel = ownerBank;
                    }
                }
                if (condition.TransactionType != null)
                {
                    TransactionHistoryType transactionT = (TransactionHistoryType)condition.TransactionType;
                    if (transactionT == TransactionHistoryType.PayInAndWithdraw)
                    {
                        transactionHistoryViewModel = transactionHistoryViewModel.Where(x => x.TransactionHistoryType == TransactionHistoryType.PayIn || x.TransactionHistoryType == TransactionHistoryType.Withdraw).ToList();
                    }
                    else
                    {
                        if(transactionT == TransactionHistoryType.ToBetAndToReward)
                        {
                            transactionHistoryViewModel = transactionHistoryViewModel.Where(x => x.TransactionHistoryType == TransactionHistoryType.ToBet || x.TransactionHistoryType == TransactionHistoryType.ToReward).ToList();
                        }
                        else
                        {
                            transactionHistoryViewModel = transactionHistoryViewModel.Where(x => x.TransactionHistoryType == transactionT).ToList();
                        }
                       
                    }

                }

                if (condition.BillStatus != null)
                {
                    BillStatus billS = (BillStatus)condition.BillStatus;
                    if(billS == BillStatus.CompletedAndCancelled)
                    {
                        transactionHistoryViewModel = transactionHistoryViewModel.Where(x => x.BillStatus == BillStatus.Completed || x.BillStatus == BillStatus.Cancelled).ToList();
                    }
                    else
                    {
                        transactionHistoryViewModel = transactionHistoryViewModel.Where(x => x.BillStatus == billS).ToList();
                    }
                    
                }


   
                return transactionHistoryViewModel;
            }
            catch (System.Exception)
            {

                throw;
            }
        }


        // PUT: api/TransactionHistories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionHistory(string id, TransactionHistory transactionHistory)
        {
            if (id != transactionHistory.Id)
            {
                return BadRequest();
            }

            _context.Entry(transactionHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionHistoryExists(id))
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

        // POST: api/TransactionHistories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TransactionHistory>> PostTransactionHistory(TransactionHistory transactionHistory)
        {
            _context.TransactionHistories.Add(transactionHistory);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TransactionHistoryExists(transactionHistory.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTransactionHistory", new { id = transactionHistory.Id }, transactionHistory);
        }

        // DELETE: api/TransactionHistories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TransactionHistory>> DeleteTransactionHistory(string id)
        {
            var transactionHistory = await _context.TransactionHistories.FindAsync(id);
            if (transactionHistory == null)
            {
                return NotFound();
            }

            _context.TransactionHistories.Remove(transactionHistory);
            await _context.SaveChangesAsync();

            return transactionHistory;
        }

        private bool TransactionHistoryExists(string id)
        {
            return _context.TransactionHistories.Any(e => e.Id == id);
        }


    }
}

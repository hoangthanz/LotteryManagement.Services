using AutoMapper;
using LotteryManagement.Application.ViewModels;
using LotteryManagement.Application.ViewModels.Conditions;
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
    public class TransactionsController : ControllerBase
    {
        private readonly LotteryManageDbContext _context;

        public TransactionsController(LotteryManageDbContext context)
        {
            _context = context;
        }

        // POST: api/Transactions
        [HttpPost("condition")]
        public async Task<ActionResult<List<TransactionViewModel>>> GetTransactions(TransactionHistoryCondition condition)
        {
            try
            {
                var transactions = _context.Transactions.Where(x=>x.Status == Status.Active).OrderByDescending(x => x.DateCreated).ToList();

                if (condition.FromDate != null)
                {
                    transactions = transactions.Where(x => x.DateCreated >= condition.FromDate && x.DateCreated <= condition.ToDate).ToList();
                }


                var transactionsViewModel = Mapper.Map<List<Transaction>, List<TransactionViewModel>>(transactions);

                foreach (var item in transactionsViewModel)
                {
                    var user = await _context.AppUsers.Where(x => x.Id == item.UserId).FirstOrDefaultAsync();
                    var userView = Mapper.Map<AppUser, AppUserViewModel>(user);
                    item.AppUserViewModel = userView;
                }
                if (condition.TransactionType != null)
                {
                    TransactionType transactionT = (TransactionType)condition.TransactionType;
                    if(transactionT == TransactionType.PayInAndWithdraw) 
                    {
                        transactionsViewModel = transactionsViewModel.Where(x => x.TransactionType == TransactionType.PayIn || x.TransactionType == TransactionType.Withdraw).ToList();
                    }
                    else
                    {
                        transactionsViewModel = transactionsViewModel.Where(x => x.TransactionType == transactionT).ToList();
                    }

                }

                if(condition.BillStatus != null)
                {
                    BillStatus billS = (BillStatus)condition.BillStatus;
                    transactionsViewModel = transactionsViewModel.Where(x => x.BillStatus == billS).ToList();
                }

                

                return transactionsViewModel;
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(string id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // PUT: api/Transactions/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(string id, Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest(new ResponseResult("Id trong giao dịch phải giống nhau"));
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return BadRequest(new ResponseResult("Không tìm thấy giao dịch!"));
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Transactions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TransactionExists(transaction.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Transaction>> DeleteTransaction(string id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        private bool TransactionExists(string id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }

        [HttpPut("confirm-transaction/{id}")]
        public async Task<ActionResult> ConfirmTransaction(string id, Transaction tran)
        {

            try
            {
                if (id != tran.Id)
                {
                    return BadRequest(new ResponseResult("Id trong giao dịch phải giống nhau"));
                }

                var transaction = _context.Transactions.Where(x => x.Id == id).FirstOrDefault();

                transaction.BillStatus = tran.BillStatus;

                if(transaction.BillStatus == BillStatus.InProgress)
                {
                    return Ok("Không có sự thay đổi?");
                }

                var user = _context.AppUsers.Where(x => x.Id == transaction.UserId).FirstOrDefault();

                // Cập nhật tiền vào ví

                var wallet = _context.Wallets.Where(x => x.WalletId == user.WalletId).FirstOrDefault();

                if (transaction.BillStatus != BillStatus.Cancelled)
                {
                  
                    //Nạp tiền
                    if (transaction.TransactionType == TransactionType.PayIn)
                    {
                        wallet.Coin += transaction.Coin;
                    }

                    //Rút tiền
                    if (transaction.TransactionType == TransactionType.Withdraw)
                    {
                        wallet.Coin -= transaction.Coin;
                    }
                }
              

                // Ghi log giao dịch
                var transactionHistoryType = (TransactionHistoryType)((int)transaction.TransactionType);
                

                var transactionHistory = new TransactionHistory
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = wallet.UserId,
                    BillStatus = tran.BillStatus,
                    Coin = Math.Abs(transaction.Coin),
                    DateCreated = DateTime.Now,
                    Status = Status.Active,
                    TransactionHistoryType = transactionHistoryType,
                };

                string notifyTransaction = " đã nạp ";
                if (transactionHistory.TransactionHistoryType == TransactionHistoryType.Withdraw)
                    notifyTransaction = " đã rút ";

                transactionHistory.Content = "Tài khoản " + user.UserName + notifyTransaction + transaction.Coin + " vào lúc " + transactionHistory.DateCreated.ToString("dd/MM/yyyy hh:mm:ss tt");

                _context.TransactionHistories.Add(transactionHistory);


                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return BadRequest(new ResponseResult("Không tìm thấy giao dịch!"));
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    }
}

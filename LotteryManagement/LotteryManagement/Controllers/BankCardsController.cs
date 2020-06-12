using AutoMapper;
using LotteryManagement.Application.ViewModels;
using LotteryManagement.Application.ViewModels.Inputs;
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
    public class BankCardsController : ControllerBase
    {
        private readonly LotteryManageDbContext _context;

        public BankCardsController(LotteryManageDbContext context)
        {
            _context = context;
        }

        // GET: api/BankCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankCard>>> GetBankCards()
        {
            return await _context.BankCards.ToListAsync();
        }

        // GET: api/BankCards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BankCard>> GetBankCard(string id)
        {
            var bankCard = await _context.BankCards.FindAsync(id);

            if (bankCard == null)
            {
                return NotFound();
            }

            return bankCard;
        }



        // GET: api/BankCards/5
        [HttpGet("get-cards-by-user/{id}")]
        public async Task<ActionResult<List<BankCardViewModel>>> GetBankCardByUser(string id)
        {

            try
            {
                var bankCards = await _context.BankCards.Where(x => id == x.UserId.ToString()).ToListAsync();

                if (bankCards == null)
                {
                    return BadRequest(new ResponseResult("Không tìm ngân hàng liên kết với tài khoản!"));
                }

                var bankCardsViewModel = Mapper.Map<List<BankCard>, List<BankCardViewModel>>(bankCards);
                return bankCardsViewModel;

            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new ResponseResult("Lỗi không xác định!"));
            }


        }

        // PUT: api/BankCards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBankCard(string id, BankCardViewModel bankCardViewModel)
        {
            if (id != bankCardViewModel.Id)
            {
                return BadRequest();
            }

            var bankCard = _context.BankCards.Find(bankCardViewModel.Id);
            bankCard.BankAccountNumber = bankCardViewModel.BankAccountNumber;
            bankCard.BankBranch = bankCardViewModel.BankBranch;
            bankCard.BankName = bankCardViewModel.BankName;
            bankCard.FullNameOwner = bankCardViewModel.FullNameOwner;

           

            try
            {

                _context.BankCards.Update(bankCard);
                await _context.SaveChangesAsync();
                return Ok(new ResponseResult("Cập nhật ngân hàng thành công!"));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankCardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(new ResponseResult("Lỗi không xác định!"));
                }
            }

            
        }

        // POST: api/BankCards
        [HttpPost]
        public async Task<ActionResult<BankCardViewModel>> PostBankCard(InputBankCard input)
        {

            try
            {
                if (input.ConfirmBankAccountNumber != input.BankAccountNumber)
                {
                    return BadRequest(new ResponseResult("Số tài khoản và xác nhận số tài khoản không trùng nhau!"));
                }

                var user = await _context.AppUsers.Where(x => x.Id.ToString() == input.UserId).FirstOrDefaultAsync();

                if (user.TransactionPassword != input.TransactionPassword)
                {
                    return BadRequest(new ResponseResult("Mật khẩu giao dịch không đúng!"));
                }


                var bankCard = new BankCard
                {
                    Id = Guid.NewGuid().ToString(),
                    BankName = input.BankName,
                    BankAccountNumber = input.BankAccountNumber,
                    BankBranch = input.BankBranch,
                    FullNameOwner = input.FullNameOwner,
                    DateCreated = DateTime.Now,
                    Status = Status.Active,
                    UserId = user.Id,
                };

                await _context.BankCards.AddAsync(bankCard);
                await _context.SaveChangesAsync();

                var bankCardViewModel = Mapper.Map<BankCard, BankCardViewModel>(bankCard);

                return CreatedAtAction("GetBankCard", new { id = bankCardViewModel.Id }, bankCardViewModel);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new ResponseResult("Lỗi không xác định!"));
            }


        }

        // DELETE: api/BankCards/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BankCard>> DeleteBankCard(string id)
        {
            var bankCard = await _context.BankCards.FindAsync(id);
            if (bankCard == null)
            {
                return NotFound();
            }

            _context.BankCards.Remove(bankCard);
            await _context.SaveChangesAsync();

            return bankCard;
        }

        private bool BankCardExists(string id)
        {
            return _context.BankCards.Any(e => e.Id == id);
        }
    }
}

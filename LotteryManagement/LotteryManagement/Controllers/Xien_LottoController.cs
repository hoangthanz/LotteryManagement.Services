﻿using LotteryManagement.Application.ViewModels;
using LotteryManagement.Application.ViewModels.Conditions;
using LotteryManagement.Data.EF;
using LotteryManagement.Data.Entities;
using LotteryManagement.Data.Enums;
using LotteryManagement.Utilities.Constants;
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
    public class Xien_LottoController : ControllerBase
    {
        private readonly LotteryManageDbContext _context;

        public Xien_LottoController(LotteryManageDbContext context)
        {
            _context = context;
        }

        

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



        [HttpPost("betting-on-xien")]
        public async Task<ActionResult<object>> Post_BettingOn_Xien_Lotto(BettingOnXien bettingOnXien)
        {

            /*
             * Nhận về là 1 string => danh sách các con số được ngăn cách nhau bởi dấu ";" or "," or " " 
             * Xử lí: Tách chuỗi để lấy ra các con đề rồi insert vào db với các tài khoản tương ứng ticket
             */

            try
            {
                if (string.IsNullOrEmpty(bettingOnXien.XienArray))
                {
                    return BadRequest(new ResponseResult("Lỗi đặt cược!, danh sách cược không thể để trống."));
                }

                // trường hợp không phải đầu cũng không phải đuôi thì xử lí bên dưới!
                var xienList = new List<string>();
                if (bettingOnXien.DivideType == DivideType.Comma)
                {
                    xienList = new List<string>(bettingOnXien.XienArray.Split(','));
                }

                if (bettingOnXien.DivideType == DivideType.SemiColon)
                {
                    xienList = new List<string>(bettingOnXien.XienArray.Split(';'));
                }

                if (bettingOnXien.DivideType == DivideType.Space)
                {
                    xienList = new List<string>(bettingOnXien.XienArray.Split(' '));
                }



                if (xienList.Count <= 1)
                {
                    return BadRequest(new ResponseResult("Chọn sai định dạng ngăn cách!"));
                }

                // loại bỏ các khoảng cách thừa và các phần tử rỗng
                xienList = xienList.Select(innerItem => innerItem?.Trim())
                    .Where(x => (
                            !string.IsNullOrEmpty(x) 
                            && !string.IsNullOrWhiteSpace(x))
                            && CheckNumber(x)
                    ).ToList();



                if (xienList.Count == 0)
                {
                    return BadRequest(new ResponseResult("Bộ sô cược của bạn chọn không phù hợp. Yêu cầu kiểm tra lại!"));
                }

                // kiểm tra xem đủ xiền để đặt với bộ số lọc đc ở trên không
                var walletOfUser = await _context.Wallets.Where(x => x.UserId == bettingOnXien.UserId.ToString()).FirstOrDefaultAsync();

                if (walletOfUser == null)
                {
                    return BadRequest(new ResponseResult("Không tìm thấy ví của bạn!"));
                }

                double feeTotal = 0;
                double fee = BettingOnPrice.LoXien;

                feeTotal = xienList.Count * bettingOnXien.MultipleNumber * fee;


                if (walletOfUser.Coin <= 0 || walletOfUser.Coin < feeTotal)
                {
                    return BadRequest(new ResponseResult("Số dư của bạn không đủ để đặt cược"));
                }

     

                // trừ tiền ở ví trước khi đặt cược

                walletOfUser.Coin -= feeTotal;

                _context.Wallets.Update(walletOfUser);
                _context.SaveChanges();


                var currentProfit = await _context.ProfitPercents.Where(x =>
                         x.Status == Status.Active
                        && x.RegionStatus == bettingOnXien.RegionStatus
                        && x.ProvincialCity == bettingOnXien.ProvincialCity
                    ).FirstOrDefaultAsync();

                var ticket = new Ticket()
                {
                    RegionStatus = currentProfit.RegionStatus,
                    Lo2So1K = currentProfit.Lo2So1K,
                    Lo2So = currentProfit.Lo2So,
                    Lo2SoDau = currentProfit.Lo2SoDau,
                    Lo3So = currentProfit.Lo3So,
                    Lo4So = currentProfit.Lo4So,
                    Cang3 = currentProfit.Cang3,
                    Dau = currentProfit.Dau,
                    Cang4 = currentProfit.Cang4,
                    DeDacBiet = currentProfit.DeDacBiet,
                    DeDauDacBiet = currentProfit.DeDauDacBiet,
                    DeGiai7 = currentProfit.DeGiai7,
                    DeGiaiNhat = currentProfit.DeGiaiNhat,
                    DeDau = currentProfit.DeDau,
                    DeDauDuoi = currentProfit.DeDauDuoi,
                    Duoi = currentProfit.Duoi,
                    TruotXien10 = currentProfit.TruotXien10,
                    TruotXien4 = currentProfit.TruotXien4,
                    TruotXien8 = currentProfit.TruotXien8,
                    Xien2 = currentProfit.Xien2,
                    Xien3 = currentProfit.Xien3,
                    Xien4 = currentProfit.Xien4,
                    UserId = bettingOnXien.UserId,
                    Status = Status.Active,
                    DateCreated = DateTime.Now,
                    De_Total = 0,
                    Bao_Total = 0,
                    Cang_Total = 0,
                    Xien_Total = feeTotal,
                    ProvincialCity = bettingOnXien.ProvincialCity,
                    Id = Guid.NewGuid().ToString()
                };


                ticket.Content = "Xiên: ";

                foreach (var item in xienList)
                {
                    ticket.Content += item + " ";
                }

                ticket.Content += "\nĐơn giá: " + fee * bettingOnXien.MultipleNumber;

                await _context.Tickets.AddAsync(ticket);
                _context.SaveChanges();

                List<Xien_Lotto> xien_Lottos = new List<Xien_Lotto>();

                foreach (var xien in xienList)
                {
                    var xienTemp = new Xien_Lotto()
                    {
                        Value = xien,
                        Xien_LottoStatus = bettingOnXien.Xien_LottoStatus,
                        RegionStatus = bettingOnXien.RegionStatus,
                        ProvincialCity = bettingOnXien.ProvincialCity,
                        Price = fee * bettingOnXien.MultipleNumber,
                        IsGoal = null,
                        DateCreated = DateTime.Now,
                        Status = Status.Active,
                        TicketId = ticket.Id,
                    };

                    xien_Lottos.Add(xienTemp);
                }

                await _context.Xien_Lottos.AddRangeAsync(xien_Lottos);
                _context.SaveChanges();


                return Ok("Đặt cược thành công!");
            }
            catch (DbUpdateException e)
            {
                return BadRequest(new ResponseResult("Lỗi không xác định! " + e.Message));
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseResult("Lỗi không xác định! " + e.Message));
            }
        }



        private bool CheckNumber(string str)
        {
        
            var checkList = new List<string>(str.Split('&'));

            foreach (var item in checkList)
            {
                if (!IsNumber(item) && item.Length == 2)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsNumber(string str) => int.TryParse(str, out int n);
    }
}

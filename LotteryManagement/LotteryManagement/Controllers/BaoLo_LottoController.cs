using LotteryManagement.Application.ViewModels;
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
    public class BaoLo_LottoController : ControllerBase
    {
        private readonly LotteryManageDbContext _context;
        public BaoLo_LottoController(LotteryManageDbContext context)
        {
            _context = context;
        }


        private bool IsNumber(string str) => int.TryParse(str, out int n);

        [HttpPost("betting-on-baolo")]
        public async Task<ActionResult<object>> Post_BettingOn_BaoLo_Lotto(BettingOnBaoLo bettingOnBaoLo)
        {

            /*
             * Nhận về là 1 string => danh sách các con số được ngăn cách nhau bởi dấu ";" or "," or " " 
             * Xử lí: Tách chuỗi để lấy ra các con đề rồi insert vào db với các tài khoản tương ứng ticket
             */

            try
            {
                if (string.IsNullOrEmpty(bettingOnBaoLo.BaoLoArray))
                {
                    return BadRequest(new ResponseResult("Lỗi đặt cược!, danh sách cược không thể để trống."));
                }

                // trường hợp không phải đầu cũng không phải đuôi thì xử lí bên dưới!
                var baoLoList = new List<string>();
                if (bettingOnBaoLo.DivideType == DivideType.Comma)
                {
                    baoLoList = new List<string>(bettingOnBaoLo.BaoLoArray.Split(','));
                }

                if (bettingOnBaoLo.DivideType == DivideType.SemiColon)
                {
                    baoLoList = new List<string>(bettingOnBaoLo.BaoLoArray.Split(';'));
                }

                if (bettingOnBaoLo.DivideType == DivideType.Space)
                {
                    baoLoList = new List<string>(bettingOnBaoLo.BaoLoArray.Split(' '));
                }



                if (baoLoList.Count <= 1)
                {
                    return BadRequest(new ResponseResult("Chọn sai định dạng ngăn cách!"));
                }

                // loại bỏ các khoảng cách thừa và các phần tử rỗng, lọc các phần tử chỉ có 2 kí tự hoặc 3 kí tự là số

                baoLoList = baoLoList.Select(innerItem => innerItem?.Trim())
                    .Where(x => (!string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
                        && (
                                (x.Length == 2 && (bettingOnBaoLo.Bao_LottoStatus == Bao_LottoStatus.Lo2So
                                || bettingOnBaoLo.Bao_LottoStatus == Bao_LottoStatus.Lo2So1K
                                || bettingOnBaoLo.Bao_LottoStatus == Bao_LottoStatus.Lo2SoDau))

                                || (x.Length == 3 && bettingOnBaoLo.Bao_LottoStatus == Bao_LottoStatus.Lo3So)
                                || (x.Length == 4 && bettingOnBaoLo.Bao_LottoStatus == Bao_LottoStatus.Lo4So)

                           )
                        && IsNumber(x)
                    ).ToList();

                if (baoLoList.Count == 0)
                {
                    return BadRequest(new ResponseResult("Bộ sô cược của bạn chọn không phù hợp. Yêu cầu kiểm tra lại!"));
                }

                // kiểm tra xem đủ xiền để đặt với bộ số lọc đc ở trên không
                var walletOfUser = await _context.Wallets.Where(x => x.UserId == bettingOnBaoLo.UserId.ToString()).FirstOrDefaultAsync();

                if (walletOfUser == null)
                {
                    return BadRequest(new ResponseResult("Không tìm thấy ví của bạn!"));
                }


                double feeTotal = 0;
                double fee = 0;

                if (RegionStatus.North == bettingOnBaoLo.RegionStatus)
                {
                    if (Bao_LottoStatus.Lo2So == bettingOnBaoLo.Bao_LottoStatus)
                    {
                        fee = BettingOnPrice.BaoLo2So_MienBac;
                    }
                    else
                    {
                        if (Bao_LottoStatus.Lo2SoDau == bettingOnBaoLo.Bao_LottoStatus
                            || Bao_LottoStatus.Lo3So == bettingOnBaoLo.Bao_LottoStatus)
                            fee = BettingOnPrice.BaoLo3So_MienBac;
                        else
                        {
                            if (Bao_LottoStatus.Lo4So == bettingOnBaoLo.Bao_LottoStatus)
                                fee = BettingOnPrice.BaoLo4So_MienBac;
                            else
                            {
                                if (Bao_LottoStatus.Lo2So1K == bettingOnBaoLo.Bao_LottoStatus)
                                    fee = BettingOnPrice.BaoLo2So1K_MienBac;
                                else
                                    return BadRequest(new ResponseResult("Lỗi sai định dạng cược!"));
                            }
                           
                        }
                    }
                }



                if (RegionStatus.Central == bettingOnBaoLo.RegionStatus)
                {
                    if (Bao_LottoStatus.Lo2So == bettingOnBaoLo.Bao_LottoStatus)
                    {
                        fee = BettingOnPrice.BaoLo2So_MienTrung;
                    }
                    else
                    {
                        if (Bao_LottoStatus.Lo3So == bettingOnBaoLo.Bao_LottoStatus)
                            fee = BettingOnPrice.BaoLo3So_MienTrung;
                        else
                        {
                            if (Bao_LottoStatus.Lo4So == bettingOnBaoLo.Bao_LottoStatus)
                                fee = BettingOnPrice.BaoLo4So_MienTrung;
                            else
                            {
                                if (Bao_LottoStatus.Lo2So1K == bettingOnBaoLo.Bao_LottoStatus)
                                    fee = BettingOnPrice.BaoLo2So1K_MienTrung;
                                else
                                    return BadRequest(new ResponseResult("Lỗi sai định dạng cược!"));
                            }

                        }
                    }
                }

                if (RegionStatus.South == bettingOnBaoLo.RegionStatus)
                {
                    if (Bao_LottoStatus.Lo2So == bettingOnBaoLo.Bao_LottoStatus)
                    {
                        fee = BettingOnPrice.BaoLo2So_MienNam;
                    }
                    else
                    {
                        if (Bao_LottoStatus.Lo3So == bettingOnBaoLo.Bao_LottoStatus)
                            fee = BettingOnPrice.BaoLo3So_MienNam;
                        else
                        {
                            if (Bao_LottoStatus.Lo4So == bettingOnBaoLo.Bao_LottoStatus)
                                fee = BettingOnPrice.BaoLo4So_MienNam;
                            else
                            {
                                if (Bao_LottoStatus.Lo2So1K == bettingOnBaoLo.Bao_LottoStatus)
                                    fee = BettingOnPrice.BaoLo2So1K_MienNam;
                                else
                                    return BadRequest(new ResponseResult("Lỗi sai định dạng cược!"));
                            }

                        }
                    }
                }

                feeTotal = baoLoList.Count * bettingOnBaoLo.MultipleNumber * fee;


                if (walletOfUser.Coin <= 0 || walletOfUser.Coin < feeTotal)
                {
                    return BadRequest(new ResponseResult("Số dư của bạn không đủ để đặt cược"));
                }

                // trừ tiền ở ví trước khi đặt cược

                walletOfUser.Coin -= feeTotal;

                _context.Wallets.Update(walletOfUser);
                _context.SaveChanges();


                var currentProfit = await _context.ProfitPercents.Where(x => x.Status == Status.Active && x.RegionStatus == bettingOnBaoLo.RegionStatus).FirstOrDefaultAsync();
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
                    UserId = bettingOnBaoLo.UserId,
                    Status = Status.Active,
                    DateCreated = DateTime.Now,
                    De_Total = 0,
                    Bao_Total = feeTotal,
                    Cang_Total = 0,
                    Xien_Total = 0,
                    Id = Guid.NewGuid().ToString()
                };

                ticket.Content = "Bao lô: ";

                foreach (var item in baoLoList)
                {
                    ticket.Content += item + " ";
                }

                ticket.Content += "\nĐơn giá: " + fee * bettingOnBaoLo.MultipleNumber;

                await _context.Tickets.AddAsync(ticket);
                _context.SaveChanges();

                List<Bao_Lotto> bao_Lottos = new List<Bao_Lotto>();

                foreach (var baoLo in baoLoList)
                {
                    var baoLoTemp = new Bao_Lotto()
                    {
                        Value = baoLo,
                        Bao_LottoStatus = bettingOnBaoLo.Bao_LottoStatus,
                        RegionStatus = bettingOnBaoLo.RegionStatus,
                        Price = fee * bettingOnBaoLo.MultipleNumber,
                        IsGoal = null,
                        DateCreated = DateTime.Now,
                        Status = Status.Active,
                        TicketId = ticket.Id,
                    };

                    bao_Lottos.Add(baoLoTemp);
                }

                await _context.Bao_Lottos.AddRangeAsync(bao_Lottos);
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
    } 
  
}
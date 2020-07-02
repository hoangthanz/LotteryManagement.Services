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
    public class Cang_LottoController : ControllerBase
    {
        private readonly LotteryManageDbContext _context;

        public Cang_LottoController(LotteryManageDbContext context)
        {
            _context = context;
        }

        // GET: api/Cang_Lotto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cang_Lotto>>> GetCang_Lottos()
        {
            return await _context.Cang_Lottos.ToListAsync();
        }

        // GET: api/Cang_Lotto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cang_Lotto>> GetCang_Lotto(string id)
        {
            var cang_Lotto = await _context.Cang_Lottos.FindAsync(id);

            if (cang_Lotto == null)
            {
                return NotFound();
            }

            return cang_Lotto;
        }

        private bool IsNumber(string str) => int.TryParse(str, out int n);

        [HttpPost("betting-on-cang")]
        public async Task<ActionResult<object>> Post_BettingOn_Cang_Lotto(BettingOnCang bettingOnCang)
        {

            /*
             * Nhận về là 1 string => danh sách các con số được ngăn cách nhau bởi dấu ";" or "," or " " 
             * Xử lí: Tách chuỗi để lấy ra các con đề rồi insert vào db với các tài khoản tương ứng ticket
             */

            try
            {
                if (string.IsNullOrEmpty(bettingOnCang.CangArray))
                {
                    return BadRequest(new ResponseResult("Lỗi đặt cược!, danh sách cược không thể để trống."));
                }

                // trường hợp không phải đầu cũng không phải đuôi thì xử lí bên dưới!
                var cangList = new List<string>();

                if (bettingOnCang.DivideType == DivideType.Comma)
                {
                    cangList = new List<string>(bettingOnCang.CangArray.Split(','));
                }

                if (bettingOnCang.DivideType == DivideType.SemiColon)
                {
                    cangList = new List<string>(bettingOnCang.CangArray.Split(';'));
                }

                if (bettingOnCang.DivideType == DivideType.Space)
                {
                    cangList = new List<string>(bettingOnCang.CangArray.Split(' '));
                }



                if (cangList.Count <= 1)
                {
                    return BadRequest(new ResponseResult("Chọn sai định dạng ngăn cách!"));
                }

                // loại bỏ các khoảng cách thừa và các phần tử rỗng
                cangList = cangList.Select(innerItem => innerItem?.Trim())
                    .Where(x =>
                        (!string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
                            && (
                                    (x.Length == 3 && bettingOnCang.Cang_LottoStatus == Cang_LottoStatus.Cang3)
                                    || (x.Length == 4 && bettingOnCang.Cang_LottoStatus == Cang_LottoStatus.Cang4)
                                )
                            && IsNumber(x)).ToList();


                if (cangList.Count == 0)
                {
                    return BadRequest(new ResponseResult("Bộ sô cược của bạn chọn không phù hợp. Yêu cầu kiểm tra lại!"));
                }

                // kiểm tra xem đủ xiền để đặt với bộ số lọc đc ở trên không
                var walletOfUser = await _context.Wallets.Where(x => x.UserId == bettingOnCang.UserId.ToString()).FirstOrDefaultAsync();

                if (walletOfUser == null)
                {
                    return BadRequest(new ResponseResult("Không tìm thấy ví của bạn!"));
                }

                double feeTotal = 0;
                double fee = (bettingOnCang.Cang_LottoStatus == Cang_LottoStatus.Cang3DauDuoi) ? BettingOnPrice.Cang3DauDuoi : BettingOnPrice.Cang3DacBiet;

                feeTotal = cangList.Count * bettingOnCang.MultipleNumber * fee;


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
                        && x.RegionStatus == bettingOnCang.RegionStatus
                        && x.ProvincialCity == bettingOnCang.ProvincialCity
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
                    UserId = bettingOnCang.UserId,
                    Status = Status.Active,
                    DateCreated = DateTime.Now,
                    De_Total = 0,
                    Bao_Total = 0,
                    Cang_Total = 0,
                    Xien_Total = feeTotal,
                    ProvincialCity = bettingOnCang.ProvincialCity,
                    Id = Guid.NewGuid().ToString()
                };


                ticket.Content = "Càng: ";

                foreach (var item in cangList)
                {
                    ticket.Content += item + " ";
                }

                ticket.Content += "\nĐơn giá: " + fee * bettingOnCang.MultipleNumber;

                await _context.Tickets.AddAsync(ticket);
                _context.SaveChanges();

                List<Cang_Lotto> cang_Lottos = new List<Cang_Lotto>();

                foreach (var cang in cangList)
                {
                    var cangTemp = new Cang_Lotto()
                    {
                        Value = cang,
                        Cang_LottoStatus = bettingOnCang.Cang_LottoStatus,
                        RegionStatus = bettingOnCang.RegionStatus,
                        ProvincialCity = bettingOnCang.ProvincialCity,
                        Price = fee * bettingOnCang.MultipleNumber,
                        IsGoal = null,
                        DateCreated = DateTime.Now,
                        Status = Status.Active,
                        TicketId = ticket.Id,
                    };

                    cang_Lottos.Add(cangTemp);
                } 

                await _context.Cang_Lottos.AddRangeAsync(cang_Lottos);
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

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

        [HttpPost("betting-on-de")]
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

                // Trường hợp là đầu hoặc đuôi
                if (bettingOnDe.SpecialDeType == SpecialDeType.Head || bettingOnDe.SpecialDeType == SpecialDeType.Tail)
                {
                    var deList = new List<string>();
                    if (bettingOnDe.DivideType == DivideType.Comma)
                    {
                        deList = new List<string>(bettingOnDe.DeArray.Split(','));
                    }

                    if (bettingOnDe.DivideType == DivideType.SemiColon)
                    {
                        deList = new List<string>(bettingOnDe.DeArray.Split(';'));
                    }

                    if (bettingOnDe.DivideType == DivideType.Space)
                    {
                        deList = new List<string>(bettingOnDe.DeArray.Split(' '));
                    }

                    

                    if (deList.Count <= 1)
                    {
                        return BadRequest(new ResponseResult("Chọn sai định dạng ngăn cách!"));
                    }

                    // loại bỏ các khoảng cách thừa và các phần tử rỗng, lọc các phần tử chỉ có 2 kí tự và là số

                    deList = deList.Select(innerItem => innerItem?.Trim())
                        .Where(x => (
                            !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))
                            && x.Length == 1
                            && IsNumber(x)
                        ).ToList();

                    if (deList.Count == 0)
                    {
                        return BadRequest(new ResponseResult("Bộ sô cược của bạn chọn không phù hợp. Yêu cầu kiểm tra lại!"));
                    }

                    // kiểm tra xem đủ xiền để đặt với bộ số lọc đc ở trên không
                    var walletOfUser = await _context.Wallets.Where(x => x.UserId == bettingOnDe.UserId.ToString()).FirstOrDefaultAsync();

                    if (walletOfUser == null)
                    {
                        return BadRequest(new ResponseResult("Không tìm thấy ví của bạn!"));
                    }

                    var feeTotal = deList.Count * bettingOnDe.MultipleNumber;


                    if (walletOfUser.Coin == 0 && walletOfUser.Coin < feeTotal)
                    {
                        return BadRequest(new ResponseResult("Số dư của bạn không đủ để đặt cược"));
                    }

                    // trừ tiền ở ví trước khi đặt cược

                    walletOfUser.Coin -= feeTotal;

                    _context.Wallets.Update(walletOfUser);
                    _context.SaveChanges();


                    var currentProfit = await _context.ProfitPercents.Where(x => x.Status == Status.Active && bettingOnDe.RegionStatus == x.RegionStatus).FirstOrDefaultAsync();


                    var ticket = new Ticket()
                    {
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
                        Duoi = currentProfit.Duoi,
                        TruotXien10 = currentProfit.TruotXien10,
                        TruotXien4 = currentProfit.TruotXien4,
                        TruotXien8 = currentProfit.TruotXien8,
                        Xien2 = currentProfit.Xien2,
                        Xien3 = currentProfit.Xien3,
                        Xien4 = currentProfit.Xien4,
                        UserId = bettingOnDe.UserId,
                        Status = Status.Active,
                        De_Total = feeTotal,
                        DateCreated = DateTime.Now,
                        Cang_Total = 0,
                        Xien_Total = 0,
                        ProvincialCity = bettingOnDe.ProvincialCity,
                        Id = Guid.NewGuid().ToString()
                    };
                    

                    ticket.Content = (bettingOnDe.SpecialDeType == SpecialDeType.Head)?"Đề đầu: ": "Đề đuôi: ";

                    foreach (var item in deList)
                    {
                        ticket.Content += item + " ";
                    }

                    ticket.Content = "Đơn giá: " + BettingOnPrice.DeDacBiet * bettingOnDe.MultipleNumber;

                    await _context.Tickets.AddAsync(ticket);
                    _context.SaveChanges();


                    List<De_Lotto> de_Lotto = new List<De_Lotto>();

                    foreach (var de in deList)
                    {
                        var deTemp = new De_Lotto()
                        {
                            Value = de,
                            De_LottoStatus = bettingOnDe.De_LottoStatus,
                            RegionStatus = bettingOnDe.RegionStatus,
                            Price = 1000 * bettingOnDe.MultipleNumber,
                            SpecialDeType = bettingOnDe.SpecialDeType,
                            ProvincialCity = bettingOnDe.ProvincialCity,
                            IsGoal = null,
                            DateCreated = DateTime.Now,
                            Status = Status.Active,
                            TicketId = ticket.Id,
                        };

                        de_Lotto.Add(deTemp);
                    }

                    await _context.De_Lottos.AddRangeAsync(de_Lotto);
                    _context.SaveChanges();
                    return Ok("Đặt cược thành công!");
                }

                if (bettingOnDe.SpecialDeType == SpecialDeType.Normal)
                {
                    // trường hợp không phải đầu cũng không phải đuôi thì xử lí bên dưới!
                    var deList = new List<string>();
                    if (bettingOnDe.DivideType == DivideType.Comma)
                    {
                        deList = new List<string>(bettingOnDe.DeArray.Split(','));
                    }

                    if (bettingOnDe.DivideType == DivideType.SemiColon)
                    {
                        deList = new List<string>(bettingOnDe.DeArray.Split(';'));
                    }

                    if (bettingOnDe.DivideType == DivideType.Space)
                    {
                        deList = new List<string>(bettingOnDe.DeArray.Split(' '));
                    }



                    if (deList.Count <= 1)
                    {
                        return BadRequest(new ResponseResult("Chọn sai định dạng ngăn cách!"));
                    }

                    // loại bỏ các khoảng cách thừa và các phần tử rỗng, lọc các phần tử chỉ có 2 kí tự và là số

                    deList = deList.Select(innerItem => innerItem?.Trim())
                        .Where(x => (
                            !string.IsNullOrEmpty(x) || !string.IsNullOrWhiteSpace(x))
                            && x.Length == 2
                            && IsNumber(x)
                        ).ToList();

                    if (deList.Count == 0)
                    {
                        return BadRequest(new ResponseResult("Bộ sô cược của bạn chọn không phù hợp. Yêu cầu kiểm tra lại!"));
                    }

                    // kiểm tra xem đủ xiền để đặt với bộ số lọc đc ở trên không

                    var walletOfUser = await _context.Wallets.Where(x => x.UserId == bettingOnDe.UserId.ToString()).FirstOrDefaultAsync();

                    if (walletOfUser == null)
                    {
                        return BadRequest(new ResponseResult("Không tìm thấy ví của bạn!"));
                    }

                    // Kiểm tra miền và phương thức cược

                    double feeTotal = 0;
                    double fee = 0;
                    if (RegionStatus.North == bettingOnDe.RegionStatus)
                    {
                        if( De_LottoStatus.DeDacBiet == bettingOnDe.De_LottoStatus
                            || De_LottoStatus.DeGiaiNhat == bettingOnDe.De_LottoStatus
                            || De_LottoStatus.DeDauDacBiet == bettingOnDe.De_LottoStatus
                            )
                        {
                            fee = BettingOnPrice.DeDacBiet;
                        }
                        else
                        {
                            if(De_LottoStatus.DeGiai7 == bettingOnDe.De_LottoStatus)
                            {
                                fee = BettingOnPrice.DeGiai7;
                            }
                            else
                            {
                                return BadRequest(new ResponseResult("Lỗi sai định dạng cược!"));
                            }
                        }
                    }

                    if (RegionStatus.Central == bettingOnDe.RegionStatus || RegionStatus.South == bettingOnDe.RegionStatus)
                    {
                        if (De_LottoStatus.DeDacBiet == bettingOnDe.De_LottoStatus
                            || De_LottoStatus.DeDau == bettingOnDe.De_LottoStatus)
                        {
                            fee = BettingOnPrice.DeDacBiet;
                        }
                        else
                        {
                            if (De_LottoStatus.DeDauDuoi == bettingOnDe.De_LottoStatus)
                            {
                                fee = BettingOnPrice.DeDauDuoi;
                            }
                            else
                            {
                                return BadRequest(new ResponseResult("Lỗi sai định dạng cược!"));
                            }
                        }
                    }
                    feeTotal = deList.Count * bettingOnDe.MultipleNumber * fee;


                    if (walletOfUser.Coin <= 0 || walletOfUser.Coin < feeTotal)
                    {
                        return BadRequest(new ResponseResult("Số dư của bạn không đủ để đặt cược"));
                    }

                    // trừ tiền ở ví trước khi đặt cược

                    walletOfUser.Coin -= feeTotal;

                    _context.Wallets.Update(walletOfUser);
                    _context.SaveChanges();



                    var currentProfit = await _context.ProfitPercents.Where(x => x.Status == Status.Active && x.RegionStatus == bettingOnDe.RegionStatus).FirstOrDefaultAsync();


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
                        UserId = bettingOnDe.UserId,
                        Status = Status.Active,
                        DateCreated = DateTime.Now,
                        De_Total = feeTotal,
                        Cang_Total = 0,
                        Xien_Total = 0,
                        Bao_Total = feeTotal,
                        Id = Guid.NewGuid().ToString()
                    };

                    ticket.Content = "Đề: ";

                    foreach (var item in deList)
                    {
                        ticket.Content += item + " ";
                    }

                    ticket.Content += "\nĐơn giá: " + fee * bettingOnDe.MultipleNumber;

                   await _context.Tickets.AddAsync(ticket);
                    _context.SaveChanges();


                    List<De_Lotto> de_Lotto = new List<De_Lotto>();

                    foreach (var de in deList)
                    {
                        var deTemp = new De_Lotto()
                        {
                            Value = de,
                            De_LottoStatus = bettingOnDe.De_LottoStatus,
                            RegionStatus = bettingOnDe.RegionStatus,
                            Price = 1000 * bettingOnDe.MultipleNumber,
                            IsGoal = null,
                            DateCreated = DateTime.Now,
                            Status = Status.Active,
                            TicketId = ticket.Id,
                            SpecialDeType = bettingOnDe.SpecialDeType
                        };

                        de_Lotto.Add(deTemp);
                    }

                    await _context.De_Lottos.AddRangeAsync(de_Lotto);
                    _context.SaveChanges();


                   
                    return Ok("Đặt cược thành công!");
                }

                return BadRequest(new ResponseResult("Lỗi! Sai kiểu đặt cược!"));

            }
            catch (DbUpdateException e)
            {
                return BadRequest(new ResponseResult("Lỗi không xác định! "+ e.Message));
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseResult("Lỗi không xác định! " + e.Message));
            }
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

  

        private bool IsNumber(string str) => int.TryParse(str, out int n);


    }
}

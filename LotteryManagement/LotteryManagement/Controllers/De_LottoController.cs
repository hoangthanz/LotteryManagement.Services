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

        // PUT: api/De_Lotto/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDe_Lotto(string id, De_Lotto de_Lotto)
        {
            if (id != de_Lotto.Id.ToString())
            {
                return BadRequest();
            }

            _context.Entry(de_Lotto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!De_LottoExists(id))
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

        // POST: api/De_Lotto
        [HttpPost]
        public async Task<ActionResult<De_Lotto>> PostDe_Lotto(De_Lotto de_Lotto)
        {
            _context.De_Lottos.Add(de_Lotto);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (De_LottoExists(de_Lotto.Id.ToString()))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDe_Lotto", new { id = de_Lotto.Id }, de_Lotto);
        }


        [HttpPost("betting-on")]
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
                            !string.IsNullOrEmpty(x) || !string.IsNullOrWhiteSpace(x))
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


                    var currentProfit = await _context.ProfitPercents.Where(x => x.Status == Status.Active).FirstOrDefaultAsync();


                    var ticket = new Ticket()
                    {
                        Lo2So1KAfter = currentProfit.Lo2So1KAfter,
                        Lo2SoAfter = currentProfit.Lo2SoAfter,
                        Lo2SoDauAfter = currentProfit.Lo2SoDauAfter,
                        Lo3SoAfter = currentProfit.Lo3SoAfter,
                        Lo4SoAfter = currentProfit.Lo4SoAfter,
                        Cang3After = currentProfit.Cang3After,
                        DauAfter = currentProfit.DauAfter,
                        Cang4After = currentProfit.Cang4After,
                        DeDacBietAfter = currentProfit.DeDacBietAfter,
                        DeDauDacBietAfter = currentProfit.DeDauDacBietAfter,
                        DeGiai7After = currentProfit.DeGiai7After,
                        DeGiaiNhatAfter = currentProfit.DeGiaiNhatAfter,
                        DuoiAfter = currentProfit.DuoiAfter,
                        TruotXien10After = currentProfit.TruotXien10After,
                        TruotXien4After = currentProfit.TruotXien4After,
                        TruotXien8After = currentProfit.TruotXien8After,
                        Xien2After = currentProfit.Xien2After,
                        Xien3After = currentProfit.Xien3After,
                        Xien4After = currentProfit.Xien4After,
                        UserId = bettingOnDe.UserId,
                        Status = Status.Active,
                        De_Total = feeTotal,
                        DateCreated = DateTime.Now,
                        Cang_Total = 0,
                        Xien_Total = 0,
                        Id = Guid.NewGuid().ToString()
                    };
                    

                    ticket.Content = (bettingOnDe.SpecialDeType == SpecialDeType.Head)?"Đề đầu: ": "Đề đuôi: ";

                    foreach (var item in deList)
                    {
                        ticket.Content += item + " ";
                    }

                    ticket.Content = "Đơn giá: " + 1000 * bettingOnDe.MultipleNumber;

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

                    var feeTotal = deList.Count * bettingOnDe.MultipleNumber*27000;


                    if (walletOfUser.Coin == 0 && walletOfUser.Coin < feeTotal)
                    {
                        return BadRequest(new ResponseResult("Số dư của bạn không đủ để đặt cược"));
                    }

                    // trừ tiền ở ví trước khi đặt cược

                    walletOfUser.Coin -= feeTotal;

                    _context.Wallets.Update(walletOfUser);
                    _context.SaveChanges();



                    var currentProfit = await _context.ProfitPercents.Where(x => x.Status == Status.Active).FirstOrDefaultAsync();


                    var ticket = new Ticket()
                    {
                        Lo2So1KAfter = currentProfit.Lo2So1KAfter,
                        Lo2SoAfter = currentProfit.Lo2SoAfter,
                        Lo2SoDauAfter = currentProfit.Lo2SoDauAfter,
                        Lo3SoAfter = currentProfit.Lo3SoAfter,
                        Lo4SoAfter = currentProfit.Lo4SoAfter,
                        Cang3After = currentProfit.Cang3After,
                        DauAfter = currentProfit.DauAfter,
                        Cang4After = currentProfit.Cang4After,
                        DeDacBietAfter = currentProfit.DeDacBietAfter,
                        DeDauDacBietAfter = currentProfit.DeDauDacBietAfter,
                        DeGiai7After = currentProfit.DeGiai7After,
                        DeGiaiNhatAfter = currentProfit.DeGiaiNhatAfter,
                        DuoiAfter = currentProfit.DuoiAfter,
                        TruotXien10After = currentProfit.TruotXien10After,
                        TruotXien4After = currentProfit.TruotXien4After,
                        TruotXien8After = currentProfit.TruotXien8After,
                        Xien2After = currentProfit.Xien2After,
                        Xien3After = currentProfit.Xien3After,
                        Xien4After = currentProfit.Xien4After,
                        UserId = bettingOnDe.UserId,
                        Status = Status.Active,
                        De_Total = feeTotal,
                        DateCreated = DateTime.Now,
                        Cang_Total = 0,
                        Xien_Total = 0,
                        Id = Guid.NewGuid().ToString()
                    };

                    ticket.Content = "Đề: ";

                    foreach (var item in deList)
                    {
                        ticket.Content += item + " ";
                    }

                    ticket.Content += "\nĐơn giá: " + 1000 * bettingOnDe.MultipleNumber;

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

        private bool De_LottoExists(string id)
        {
            return _context.De_Lottos.Any(e => e.Id.ToString() == id);
        }


        private bool IsNumber(string str) => int.TryParse(str, out int n);


    }
}

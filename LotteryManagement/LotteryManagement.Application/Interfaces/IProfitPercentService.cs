using LotteryManagement.Application.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LotteryManagement.Application.Interfaces
{
    public interface IProfitPercentService
    {
        ProfitPercentViewModel Add(ProfitPercentViewModel profit);
       
        void Update(ProfitPercentViewModel profit);

        void Delete(string id);

        void SaveChanges();

        Task<List<ProfitPercentViewModel>> GetProfitPercents();

        Task<ProfitPercentViewModel> GetProfitPercentById(string id);
    }
}

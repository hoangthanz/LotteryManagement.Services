using AutoMapper;
using AutoMapper.QueryableExtensions;
using LotteryManagement.Application.Interfaces;
using LotteryManagement.Application.ViewModels;
using LotteryManagement.Data.Entities;
using LotteryManagement.Data.Enums;
using LotteryManagement.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LotteryManagement.Application.Implementation
{
    public class ProfitPercentService : IProfitPercentService
    {
        private IRepository<ProfitPercent, string> _profitPercentRepository;
        private IUnitOfWork _unitOfWork;

        public ProfitPercentService(
            IRepository<ProfitPercent, string> profitPercentRepository,
            IUnitOfWork unitOfWork
            )
        {
            _profitPercentRepository = profitPercentRepository;
            _unitOfWork = unitOfWork;
        }

        public ProfitPercentViewModel Add(ProfitPercentViewModel profitPercentView)
        {
            var profitPercent = Mapper.Map<ProfitPercentViewModel, ProfitPercent>(profitPercentView);

            profitPercent.Id = Guid.NewGuid().ToString();
            profitPercent.Name = "Bảng " + new Random().Next(999);
            profitPercent.Status = Status.Active;
            profitPercent.IsUsing = false;
            _profitPercentRepository.Add(profitPercent);

            return profitPercentView;
        }

        public void Delete(string id)
        {
            _profitPercentRepository.Remove(id);
        }

        public Task<ProfitPercentViewModel> GetProfitPercentById(string id)
        {
            var profit = _profitPercentRepository.FindById(id);
            var profitView = Mapper.Map<ProfitPercent, ProfitPercentViewModel>(profit);
            return Task.FromResult(profitView);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProfitPercentViewModel profitPercentView)
        {
            var profit = Mapper.Map<ProfitPercentViewModel, ProfitPercent>(profitPercentView);
            _profitPercentRepository.Update(profit);

        }

        Task<List<ProfitPercentViewModel>> IProfitPercentService.GetProfitPercents()
        {
            return _profitPercentRepository.FindAll(x => x.Status != Status.InActive).ProjectTo<ProfitPercentViewModel>().ToListAsync();
        }
    }
}

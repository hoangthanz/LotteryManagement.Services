using AutoMapper;
using LotteryManagement.Application.ViewModels;
using LotteryManagement.Data.Entities;

namespace LotteryManagement.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<AppRole, AppRoleViewModel>();
            CreateMap<AppUser, AppUserViewModel>();

            CreateMap<Contact, ContactViewModel>();

            CreateMap<Feedback, FeedbackViewModel>();



            CreateMap<Announcement, AnnouncementViewModel>().MaxDepth(2);
            CreateMap<ProfitPercent, ProfitPercentViewModel>();

            CreateMap<Wallet, WalletViewModel>();
            CreateMap<TransactionHistory, TransactionHistoryViewModel>();
            CreateMap<Transaction, TransactionViewModel>();
            CreateMap<BankCard, BankCardViewModel>();
            CreateMap<Ticket, TicketViewModel>();


        }
    }
}

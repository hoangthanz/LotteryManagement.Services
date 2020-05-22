using AutoMapper;
using LotteryManagement.Application.ViewModels;
using LotteryManagement.Data.Entities;

namespace LotteryManagement.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {

            CreateMap<AppRoleViewModel, AppRole>();
            CreateMap<AppUserViewModel, AppUser>();


            CreateMap<ContactViewModel, Contact>();


            CreateMap<FeedbackViewModel, Feedback>();

            CreateMap<ProfitPercentViewModel, ProfitPercent>();


            CreateMap<AnnouncementViewModel, Announcement>()
                .ConstructUsing(c => new Announcement(c.Title, c.Content, c.UserId, c.Status));

            CreateMap<AnnouncementUserViewModel, AnnouncementUser>()
                .ConstructUsing(c => new AnnouncementUser(c.AnnouncementId, c.UserId, c.HasRead));

        }
    }
}

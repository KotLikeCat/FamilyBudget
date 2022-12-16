using AutoMapper;
using FamilyBudget.Common.Models.Data;
using FamilyBudget.Common.Models.Input;
using FamilyBudget.Common.Models.View;

namespace FamilyBudget.Api.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserInputModel, User>();

        CreateMap<User, UserViewModel>()
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.CreateTime.ToString("G"))
            )
            .ForMember(
                dest => dest.LastLoginAt,
                opt =>
                {
                    opt.PreCondition(user => user.LastLoginTime != null);
                    opt.MapFrom(src => src.LastLoginTime!.Value.ToString("G"));
                });

        CreateMap<BaseListViewModel<User>, BaseListViewModel<UserViewModel>>();

        CreateMap<Budget, BudgetViewModel>()
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.MapFrom(src => src.CreateTime.ToString("G"))
            )
            .ForMember(
                dest => dest.Owner,
                opt => opt.MapFrom(src => src.User.Login)
            )
            .AfterMap((source, destination) =>
            {
                destination.UserIds = source.BudgetUsers.Select(x => x.UserId).ToList();
            });
        
        CreateMap<BaseListViewModel<Budget>, BaseListViewModel<BudgetViewModel>>();

        CreateMap<Guid, BudgetUser>()
            .ForMember(
                dest => dest.UserId,
                opt => opt.MapFrom(src => src)
            );
        
        CreateMap<BudgetInputModel, Budget>()
            .ForMember(
                dest => dest.BudgetUsers,
                opt => opt.MapFrom(src => src.UserIds)
            );

        CreateMap<Category, CategoryViewModel>();
        CreateMap<BaseListViewModel<Category>, BaseListViewModel<CategoryViewModel>>();
        CreateMap<CategoryInputModel, Category>();

        CreateMap<BudgetDetail, BudgetDetailViewModel>()
            .ForMember(
                dest => dest.Owner,
                opt => opt.MapFrom(src => src.User.Login)
            )
            .ForMember(
                dest => dest.Category,
                opt => opt.MapFrom(src => src.Category.Name)
            );
        CreateMap<BaseListViewModel<BudgetDetail>, BaseListViewModel<BudgetDetailViewModel>>();
        CreateMap<BudgetDetailInputModel, BudgetDetail>();
    }
}
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
    }
}
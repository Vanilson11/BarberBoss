using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Application.AutoMapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EnityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestRevenuesJson, Revenue>();
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, config => config.Ignore())
            .ForMember(dest => dest.UpdatedAt, config => config.Ignore());
        CreateMap<RequestUpdateUserJson, User>();
    }

    private void EnityToResponse()
    {
        CreateMap<Revenue, ResponseShortRevenuesJson>();
        CreateMap<Revenue, ResponseRevenueJson>();
        CreateMap<Revenue, ResponseRegisterRevenueJson>();
        CreateMap<User, ResponseProfileUserJson>();
    }
}

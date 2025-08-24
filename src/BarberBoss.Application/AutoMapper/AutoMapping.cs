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
    }

    private void EnityToResponse()
    {
        CreateMap<Revenue, ResponseShortRevenuesJson>();
        CreateMap<Revenue, ResponseRevenueJson>();
    }
}

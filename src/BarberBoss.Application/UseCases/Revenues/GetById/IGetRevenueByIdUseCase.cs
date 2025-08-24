using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Revenues.GetById;
public interface IGetRevenueByIdUseCase
{
    Task<ResponseRevenueJson> Execute(long id);
}

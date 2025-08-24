using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Revenues.GetAll;
public interface IGetAllRevenuesUseCase
{
    Task<ResponseRevenuesJson> Execute();
}

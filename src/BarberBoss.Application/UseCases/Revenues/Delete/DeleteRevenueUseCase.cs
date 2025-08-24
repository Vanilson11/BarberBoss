
using BarberBoss.Domain.Repositories;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Revenues.Delete;
public class DeleteRevenueUseCase : IDeleteRevenueUseCase
{
    private readonly IWriteOnlyRevenueRepository _repository;
    private readonly IUnitOffWork _unitOffWork;

    public DeleteRevenueUseCase(IWriteOnlyRevenueRepository repository, IUnitOffWork unitOffWork)
    {
        _repository = repository;
        _unitOffWork = unitOffWork;
    }
    public async Task Execute(long id)
    {
        var result = await _repository.Delete(id);

        if (result is false)
            throw new NotFoundException(ResourceErrorMessages.REVENUE_NOT_FOUND);

        await _unitOffWork.Commit();
    }
}

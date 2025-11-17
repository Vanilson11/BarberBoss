
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Revenues.Delete;
public class DeleteRevenueUseCase : IDeleteRevenueUseCase
{
    private readonly IWriteOnlyRevenueRepository _writeOnlyRepository;
    private readonly IReadOnlyRevenueRepository _readOnlyRepository;
    private readonly IUnitOffWork _unitOffWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteRevenueUseCase(
        IWriteOnlyRevenueRepository writeOnlyRepository, 
        IUnitOffWork unitOffWork,
        IReadOnlyRevenueRepository readOnlyRepository,
        ILoggedUser loggedUser)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _unitOffWork = unitOffWork;
        _readOnlyRepository = readOnlyRepository;
        _loggedUser = loggedUser;
    }
    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var revenue = await _readOnlyRepository.GetById(loggedUser, id);

        if (revenue is null)
            throw new NotFoundException(ResourceErrorMessages.REVENUE_NOT_FOUND);

        await _writeOnlyRepository.Delete(id);

        await _unitOffWork.Commit();
    }
}

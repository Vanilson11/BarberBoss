
using BarberBoss.Domain.Repositories;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Users.Delete;

public class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly IWriteOnlyUsersRepository _writeOnlyUsersRepository;
    private readonly IUnitOffWork _unitOfWork;

    public DeleteUserUseCase(
        IWriteOnlyUsersRepository writeOnlyUsersRepository,
        IUnitOffWork unitOfWork
        )
    {
        _writeOnlyUsersRepository = writeOnlyUsersRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(long id)
    {
        var user = await _writeOnlyUsersRepository.Delete(id);

        if (user is false)
        {
            throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
        }

        await _unitOfWork.Commit();
    }
}

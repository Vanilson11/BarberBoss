using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Security.Criptography;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.DoLogin;
public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IReadOnlyUsersRepository _readOnlyUsersRepository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public DoLoginUseCase(
        IReadOnlyUsersRepository readOnlyUsersRepository,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _passwordEncripter = passwordEncripter;
        _readOnlyUsersRepository = readOnlyUsersRepository;
        _accessTokenGenerator = accessTokenGenerator;
    }
    public async Task<ResponseRegisterUserJson> Execute(RequestDoLoginJson request)
    {
        var user = await _readOnlyUsersRepository.GetUserByEmail(request.Email);

        if(user is null)
        {
            throw new InvalidLoginException(ResourceErrorMessages.INVALID_LOGIN);
        }

        var passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

        if(passwordMatch is false)
        {
            throw new InvalidLoginException(ResourceErrorMessages.INVALID_LOGIN);
        }

        return new ResponseRegisterUserJson()
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user)
        };
    }
}

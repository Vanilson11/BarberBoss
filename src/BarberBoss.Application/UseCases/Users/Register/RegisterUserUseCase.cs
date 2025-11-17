using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Security.Criptography;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace BarberBoss.Application.UseCases.Users.Register;
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IReadOnlyUsersRepository _userReadOnlyRepository;
    private readonly IWriteOnlyUsersRepository _userWriteOnlyRepository;
    private readonly IUnitOffWork _unitOffWork;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public RegisterUserUseCase(
        IMapper mapper, IPasswordEncripter passwordEncripter, IReadOnlyUsersRepository userReadOnlyRepository,
        IWriteOnlyUsersRepository userWriteOnlyRepository, IUnitOffWork unitOffWork,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _unitOffWork = unitOffWork;
        _accessTokenGenerator = accessTokenGenerator;
    }
    public async Task<ResponseRegisterUserJson> Execute(RequestUserJson request)
    {
        await Validate(request);

        var user = _mapper.Map<User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();

        await _userWriteOnlyRepository.Add(user);

        await _unitOffWork.Commit();

        return new ResponseRegisterUserJson()
        {
            Name = user.Name,
            Token = _accessTokenGenerator.Generate(user)
        };
    }

    private async Task Validate(RequestUserJson request) { 
        var result = new UserUseCaseValidator().Validate(request);
        var emailExists = await _userReadOnlyRepository.EmailExists(request.Email);

        if (emailExists)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if(result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(errorMessage => errorMessage.ErrorMessage).ToList();
            //var errorResponse = new ResponseErrorMessagesJson(errorMessages);

            throw new ErrorsOnValidationException(errorMessages);
        }
    }
}

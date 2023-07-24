using BuberDinner.Application.Common.Interfaces.Persistence;
using ErrorOr;
using MediatR;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Domain.UserAggregate;
using BuberDinner.Application.Authentication.Common;

namespace BuberDinner.Application.Authentication.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IJwtTokenGenerator tokenGenerator, IUserRepository userRepository)
    {
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
    }

    public Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken
    )
    {
        // check if user exists
        if (_userRepository.GetUserByEmail(command.Email) is not null)
        {
            return Task.FromResult<ErrorOr<AuthenticationResult>>(Errors.User.UserAlreadyExists);
        }
        // create user (unique id) and Persist in Db
        var user = new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            Password = command.Password
        };

        _userRepository.Add(user);

        // generate token
        var token = _tokenGenerator.GenerateToken(user);
        return Task.FromResult<ErrorOr<AuthenticationResult>>(
            new AuthenticationResult(user, token)
        );
    }
}

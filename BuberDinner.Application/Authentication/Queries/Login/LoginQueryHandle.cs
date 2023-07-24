using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Authentication.Common;
using ErrorOr;
using MediatR;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.UserAggregate;

namespace BuberDinner.Application.Authentication.Queries.Login;

public class LoginHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginHandler(IJwtTokenGenerator tokenGenerator, IUserRepository userRepository)
    {
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
    }

    public Task<ErrorOr<AuthenticationResult>> Handle(
        LoginQuery query,
        CancellationToken cancellationToken
    )
    {
        // check if user exists
        if (_userRepository.GetUserByEmail(query.Email) is not User user)
        {
            return Task.FromResult<ErrorOr<AuthenticationResult>>(
                Errors.Authentication.InvalidCredentials
            );
        }

        // check password
        if (user.Password != query.Password)
        {
            return Task.FromResult<ErrorOr<AuthenticationResult>>(
                Errors.Authentication.InvalidCredentials
            );
        }

        // generate token
        var token = _tokenGenerator.GenerateToken(user);

        // set token
        return Task.FromResult<ErrorOr<AuthenticationResult>>(
            new AuthenticationResult(user, token)
        );
    }
}

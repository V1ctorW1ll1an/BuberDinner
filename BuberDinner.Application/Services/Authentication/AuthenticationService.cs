using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Common.Errors;
using ErrorOr;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator tokenGenerator, IUserRepository userRepository)
    {
        _tokenGenerator = tokenGenerator;
        _userRepository = userRepository;
    }

    public ErrorOr<AuthenticationResult> Login(string email, string password)
    {
        // check if user exists
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        // check password
        if (user.Password != password)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        // generate token
        var token = _tokenGenerator.GenerateToken(user);

        // set token
        return new AuthenticationResult(user, token);
    }

    public ErrorOr<AuthenticationResult> Register(
        string firstName,
        string lastName,
        string email,
        string password
    )
    {
        // check if user exists
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            return Errors.User.EmailAlreadyExists;
        }
        // create user (unique id) and Persist in Db
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        _userRepository.Add(user);

        // generate token
        var token = _tokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }
}

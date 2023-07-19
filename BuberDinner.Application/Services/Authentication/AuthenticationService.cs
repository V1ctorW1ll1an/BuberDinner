using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;
using OneOf;

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

    public AuthenticationResult Login(string email, string password)
    {
        // check if user exists
        if (_userRepository.GetUserByEmail(email) is not User user)
        {
            throw new Exception("Invalid email or password");
        }

        // check password
        if (user.Password != password)
        {
            throw new Exception("Invalid email or password");
        }

        // generate token
        var token = _tokenGenerator.GenerateToken(user);

        // set token
        return new AuthenticationResult(user, token);
    }

    public OneOf<AuthenticationResult, IError> Register(
        string firstName,
        string lastName,
        string email,
        string password
    )
    {
        // check if user exists
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            return new UserAlreadyExistsError();
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

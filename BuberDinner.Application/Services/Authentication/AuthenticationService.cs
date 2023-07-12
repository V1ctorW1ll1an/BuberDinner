using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IUserRepository _userRepoitory;

    public AuthenticationService(IJwtTokenGenerator tokenGenerator, IUserRepository userRepository)
    {
        _tokenGenerator = tokenGenerator;
        _userRepoitory = userRepository;
    }

    public AuthenticationResult Login(string email, string password)
    {
        return new AuthenticationResult(Guid.NewGuid(), "John", "Doe", "john@teste.com", "token");
    }

    public AuthenticationResult Register(
        string firstName,
        string lastName,
        string email,
        string password
    )
    {
        // check if user exists
        // create user (unique id)
        // generate token
        var userId = Guid.NewGuid();
        var token = _tokenGenerator.GenerateToken(userId, firstName, lastName);
        return new AuthenticationResult(userId, firstName, lastName, email, token);
    }
}

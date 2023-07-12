using BuberDinner.Application.Common.Interfaces.Authentication;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _tokenGenerator;

    public AuthenticationService(IJwtTokenGenerator tokenGenerator)
    {
        _tokenGenerator = tokenGenerator;
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

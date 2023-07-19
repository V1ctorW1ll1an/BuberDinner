using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using BuberDinner.Domain.Common.Errors;

namespace BuberDinner.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        ErrorOr.ErrorOr<AuthenticationResult> result = _authenticationService.Register(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password
        );

        return result.Match(authResult => Ok(MapAuthResult(authResult)), errors => Problem(errors));
    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.Token
        );
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var result = _authenticationService.Login(request.Email, request.Password);

        if (result.IsError && result.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: result.FirstError.Description
            );
        }

        return result.Match(authResult => Ok(MapAuthResult(authResult)), errors => Problem(errors));
    }
}

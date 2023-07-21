using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using BuberDinner.Domain.Common.Errors;
using MediatR;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using MapsterMapper;

namespace BuberDinner.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        //var command = new RegisterCommand(
        //    request.FirstName,
        //    request.LastName,
        //    request.Email,
        //    request.Password
        //);
        var command = _mapper.Map<RegisterCommand>(request);
        ErrorOr.ErrorOr<AuthenticationResult> result = await _mediator.Send(command);

        return result.Match(authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)), errors => Problem(errors));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        //var query = new LoginQuery(
        //    request.Email,
        //    request.Password
        //);
        var query = _mapper.Map<LoginQuery>(request);
        ErrorOr.ErrorOr<AuthenticationResult> result = await _mediator.Send(query);

        if (result.IsError && result.FirstError == Errors.Authentication.InvalidCredentials)
        {
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: result.FirstError.Description
            );
        }

        return result.Match(authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)), errors => Problem(errors));
    }
}

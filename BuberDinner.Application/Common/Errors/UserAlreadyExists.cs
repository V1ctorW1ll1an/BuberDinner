using System.Net;

namespace BuberDinner.Application.Common.Errors;

public record struct UserAlreadyExistsError : IError
{
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

    public string ErrorMessage => "User already exists";
}

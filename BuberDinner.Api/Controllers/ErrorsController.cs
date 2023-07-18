using BuberDinner.Application.Common.Interfaces.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

public class ErrorsController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        var (StatusCode, message) = exception switch
        {
            IServiceException serviceException
                => ((int)serviceException.StatusCode, serviceException.ErrorMessage),
            _ => (500, "Internal Server Error")
        };

        return Problem(title: message, statusCode: StatusCode);
    }
}

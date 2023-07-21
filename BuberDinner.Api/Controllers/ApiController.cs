using BuberDinner.Api.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BuberDinner.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
            return Problem();
        
        if (errors.All(error => error.Type == ErrorType.Validation))
            return ValidationProblem(errors);

        // custom validation
        //if (errors.All(error => error.NumericType == 23))
        //    return ValidationProblem();
        
        HttpContext.Items[HttpContextItemKeys.Errors] = errors;

        return Problem(errors[0]);
    }

    private IActionResult Problem(Error Error)
    {
        var statusCode = Error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(title: Error.Description, statusCode: statusCode);
    }

    private IActionResult ValidationProblem(List<Error> errors)
    {
        var modelStateDictionary = new ModelStateDictionary();

        foreach (var e in errors)
        {
            modelStateDictionary.AddModelError(
                e.Code,
                e.Description);
        }

        return ValidationProblem(modelStateDictionary);
    }
}

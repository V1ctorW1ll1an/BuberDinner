using System.Net;

namespace BuberDinner.Application.Common.Interfaces.Errors
{
    public class UserAlreadyExists : Exception, IServiceException
    {
        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public string ErrorMessage => "User already exists";
    }
}

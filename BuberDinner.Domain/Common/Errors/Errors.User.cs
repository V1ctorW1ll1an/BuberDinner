using ErrorOr;

namespace BuberDinner.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error UserAlreadyExists =>
            Error.Conflict(code: "User.UserAlreadyExists", description: "User already exists");
    }
}

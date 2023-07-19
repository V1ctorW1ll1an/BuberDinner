using ErrorOr;

namespace BuberDinner.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error EmailAlreadyExists =>
            Error.Conflict(code: "User.EmailAlreadyExists", description: "Email already exists");
    }
}

using System;

namespace blogger_clone.Exception.Auth;

public class UserNotFoundException : System.Exception
{
    public UserNotFoundException(Guid userId) : base (
        $"User {userId} not existed"
    ) {}
}

using System;

namespace blogger_clone.Exception.Auth;

public class InvalidCredentialException : System.Exception
{
    public InvalidCredentialException() : base (
        $"Invalid username or password"
    ){}
}

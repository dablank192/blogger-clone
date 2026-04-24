using System;

namespace blogger_clone.Exception.Auth;

public class EmailExistedException : System.Exception
{
    public EmailExistedException () : base (
        $"An account is already linked to this email"
    ) {}
}

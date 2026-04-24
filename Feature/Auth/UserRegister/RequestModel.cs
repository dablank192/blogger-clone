using System;

namespace blogger_clone.Feature.Auth.UserRegister;

public record RequestModel (
    string Email,
    string Password
);

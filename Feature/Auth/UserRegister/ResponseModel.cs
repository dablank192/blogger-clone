namespace blogger_clone.Feature.Auth.UserRegister;

public record ResponseModel (
    Guid UserId,
    string Message
);

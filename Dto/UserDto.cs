using System;

namespace blogger_clone.Dto;

public record UserDto(
    Guid UserId,
    string Username,
    string Email
);

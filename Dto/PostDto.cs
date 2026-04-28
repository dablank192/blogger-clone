using System;

namespace blogger_clone.Dto;

public record PostDto(
    Guid Id,
    string Title,
    string Content
);

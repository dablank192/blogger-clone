using System;

namespace blogger_clone.Dto;

public record PostDto(
    Guid Id,
    string Title,
    DateTime CreatedAt,
    string Content
);

public record PostDtoOwner(
    Guid Id,
    string Title,
    PostStatusDto Status,
    DateTime CreatedAt,
    string Content
);

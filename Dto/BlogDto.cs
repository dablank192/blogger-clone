using System;

namespace blogger_clone.Dto;

public record BlogDto(
    Guid BlogId,
    string SubDomain,
    string Domain
);

public record BlogDtoAdmin(
    Guid UserId,
    Guid BlogId,
    string SubDomain,
    string Domain
);
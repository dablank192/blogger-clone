using System;

namespace blogger_clone.Dto;

public record BlogDto(
    Guid BlogId,
    string SubDomain,
    string Domain
);

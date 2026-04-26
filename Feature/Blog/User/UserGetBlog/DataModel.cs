using System;
using blogger_clone.Dto;
using MediatR;

namespace blogger_clone.Feature.Blog.UserGetBlog;

public record Query() : IRequest<Result>;
public record Result(
    IEnumerable<BlogDto> Data
);

public class DataModel
{

}

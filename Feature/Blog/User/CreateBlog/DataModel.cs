using System;
using MediatR;

namespace blogger_clone.Feature.Blog.CreateBlog;


public record Query() : IRequest<Result>;
public record Result(
    Guid UserId,
    string SubDomain
);
public class DataModel
{

}

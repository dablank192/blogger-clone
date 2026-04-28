using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace blogger_clone.Feature.Post.CreatePost;

public record Command(
    [property: Required(ErrorMessage = "Title must not be empty")]
    [property: MaxLength(200, ErrorMessage = "Title text limit is 200 words")]
    string Title,

    string Content
) : IRequest<Result>;

public record Result(
    Guid PostId,
    string Message
);

public class DataModel
{

}

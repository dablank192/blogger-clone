using System;
using blogger_clone.Dto;


namespace blogger_clone.Model;

public class Post
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public int BlogId {get; set;}
    public string Content {get; set;} = default!;
    public PostStatusDto Status {get; set;} = PostStatusDto.Public;

    public Blog? Blog {get; set;}
}

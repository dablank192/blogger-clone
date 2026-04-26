using System;

namespace blogger_clone.Exception.Blog;

public class BlogExistedException : System.Exception
{
    public BlogExistedException () : base (
        $"User already have a blog"
    ){}
}

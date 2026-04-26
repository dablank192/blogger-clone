using System;

namespace blogger_clone.Exception.Blog;

public class BlogNotExistedException : System.Exception
{
    public BlogNotExistedException() : base (
        $"Blog not found"
    ) {}
}

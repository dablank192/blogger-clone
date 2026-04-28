using System;

namespace blogger_clone.Exception.Post;

public class PostNotExistedException : System.Exception
{
    public PostNotExistedException () : base (
        $"Post not existed"
    ){}
}

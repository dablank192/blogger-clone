using System;

namespace blogger_clone.Model;

public class Blog
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public Guid UserId {get; set;}
    public string SubDomain {get; set;} = default!;

    public List<Post> Post {get; set;}
    public User? User {get; set;}
}

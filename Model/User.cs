using System;

namespace blogger_clone.Model;

public class User
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public string Email {get; set;} = default!;
    public string Username {get; set;} = default!;
    public string Password {get; set;} = default!;

    public List<Blog> Blog {get; set;}
}

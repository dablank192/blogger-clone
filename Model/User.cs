using System;
using blogger_clone.Dto;

namespace blogger_clone.Model;

public class User
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public string Email {get; set;} = default!;
    public string Username {get; set;} = default!;
    public string Password {get; set;} = default!;
    public UserRolesDto Role {get; set;} = UserRolesDto.User;

    public ICollection<Blog> Blog {get; set;} = new List<Blog>();
}

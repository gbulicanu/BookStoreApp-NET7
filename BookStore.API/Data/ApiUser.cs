using System;
using Microsoft.AspNetCore.Identity;

namespace BookStore.API.Data;

public class ApiUser : IdentityUser 
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}


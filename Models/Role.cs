using Microsoft.AspNetCore.Identity;

public class Role : IdentityRole<int>{
    public ICollection<User> Users { get; set; }
}
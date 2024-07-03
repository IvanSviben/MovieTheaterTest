using System.Globalization;
using Microsoft.AspNetCore.Identity;

public static class DbInitializer{
    public static async Task Initialize(AppDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager){
        context.Database.EnsureCreated();

        // if (context.Users.Any()){
        //     return;
        // }

        if(!await roleManager.RoleExistsAsync("Admin")){
            await roleManager.CreateAsync(new Role{ Name = "Admin" });
        }
        
        if(!await roleManager.RoleExistsAsync("User")){
            await roleManager.CreateAsync(new Role{ Name = "User" });
        }

        // if(!roleManager.Roles.Any()){
        //     var adminRole = new Role{ Name = "Admin" };
        //     roleManager.CreateAsync(adminRole).Wait();

        //     var userRole = new Role{ Name = "User" };
        //     roleManager.CreateAsync(userRole).Wait();
        //}

        // if(!userManager.Users.Any()){
        //     var adminUser = new User{ UserName = "admin", Email = "admin@example.com",RoleId = 1 };
        //     userManager.CreateAsync(adminUser, "test1234").Wait();
        //     userManager.AddToRoleAsync(adminUser, "Admin").Wait();

        //     var regularUser = new User{ UserName = "user", Email = "user@example.com",RoleId = 2 };
        //     userManager.CreateAsync(regularUser, "test1234").Wait();
        //     userManager.AddToRoleAsync(regularUser, "User").Wait();
        // }
        if(await userManager.FindByNameAsync("admin") == null){

            var adminUser = new User{UserName = "Admin", Email = "admin@example.com" };
            var result = await userManager.CreateAsync(adminUser, "test1234");

            if (result.Succeeded){
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        if(await userManager.FindByNameAsync("user") == null){

            var regularUser = new User{UserName = "User", Email = "user@example.com" };
            var result = await userManager.CreateAsync(regularUser, "test1234");

            if (result.Succeeded){
                await userManager.AddToRoleAsync(regularUser, "User");
            }
        }

        if(!context.Categories.Any()){
            context.Categories.AddRange(new List<Category>{

                new Category{ Name = "Action" },
                new Category{ Name = "Comedy" },
                new Category{ Name = "Horror" }
            });
            await context.SaveChangesAsync();
        }

    }
}
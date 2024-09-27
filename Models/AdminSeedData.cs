using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserOperations;
using UserOperations.Models;

namespace UserOperations.Models
{
    public static class AdminSeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Admin_123";
        private const string adminRole = "Admin";
        private const string adminDepartment = "Bilgi İşlem";

        public static async void AdminUser(IApplicationBuilder app)
{
    using (var scope = app.ApplicationServices.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<UserOperationsContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(new AppRole { Name = adminRole });
        }

        var user = await userManager.FindByNameAsync(adminUser);
        if (user == null)
        {
            user = new AppUser
            {
                UserName = adminUser,
                Email = "admin@user.com",
                Department = adminDepartment,
            };

            var result = await userManager.CreateAsync(user, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, adminRole);
            }
        }
        else
        {
            if (!await userManager.IsInRoleAsync(user, adminRole))
            {
                await userManager.AddToRoleAsync(user, adminRole);
            }
        }
    }
}

    }}

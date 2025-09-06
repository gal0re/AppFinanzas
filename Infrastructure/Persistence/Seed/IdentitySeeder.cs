using Microsoft.AspNetCore.Identity;
using AppFinanzas.Security;

namespace AppFinanzas.Infrastructure.Persistence.Seed
{
    public static class IdentitySeeder
    {
        public static async Task RunAsync(RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMgr)
        {
            const string adminRole = "Admin";
            if (!await roleMgr.RoleExistsAsync(adminRole))
                await roleMgr.CreateAsync(new IdentityRole(adminRole));

            var email = "admin@test.com";
            var user = await userMgr.FindByEmailAsync(email);
            if (user is null)
            {
                user = new AppUser { UserName = email, Email = email, EmailConfirmed = true };
                var created = await userMgr.CreateAsync(user, "Admin123!");
                if (created.Succeeded)
                    await userMgr.AddToRoleAsync(user, adminRole);
            }
        }
    }
}

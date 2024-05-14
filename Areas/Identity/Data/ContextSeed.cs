/*
Topic Name: Electronics Store
Project Name: PcPulse
Group Name: SoftByte
Group Members:
    Shree Dhar Acharya: 8899288
    Karamjot Singh: 8869324
    Prashant Sahu: 8877584
    Jovan Sandhu: 8873660

Description:
- ContextSeed: Provides methods to seed roles in the Identity database, ensuring that required roles exist for the application.
*/


using Microsoft.AspNetCore.Identity;

namespace PcPulse.Areas.Identity.Data
{
    public static class ContextSeed
    {

        public static async Task seedRolesAsync(RoleManager<IdentityRole> roleManager)
        {

            string[] roleNames = { "SuperAdmin", "User" };

            foreach (var roleName in roleNames)
            {
                // Checking if the role already exists.
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    // Creating the role if it doesn't exist.
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

        }


    }
}

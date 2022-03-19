using Microsoft.AspNetCore.Identity;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Data
{
   
        public static class UserAndRoleDataInitializer
        {
            public static void SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                SeedRoles(roleManager);
                SeedUsers(userManager);
            }

            private static void SeedUsers(UserManager<IdentityUser> userManager)
            {
                if (userManager.FindByNameAsync("Guest").Result == null)
                {
                    ApplicationUser user = new ApplicationUser();
                    user.UserName = "Guest";
                    user.Email = "johndoe@localhost";
                    user.FirstName = "Default Guest";
                    user.LastName = "DG";

                    IdentityResult result = userManager.CreateAsync(user, "Abc@123").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "User").Wait();
                    }
                }


                if (userManager.FindByEmailAsync("alex@localhost").Result == null)
                {
                    ApplicationUser user = new ApplicationUser();
                    user.UserName = "alex@localhost";
                    user.Email = "alex@localhost";
                    user.FirstName = "Alex";
                    user.LastName = "Calingasan";

                    IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd1!").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
                }
            }

            private static void SeedRoles(RoleManager<IdentityRole> roleManager)
            {
                if (!roleManager.RoleExistsAsync("User").Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = "User";
                    IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
                }


                if (!roleManager.RoleExistsAsync("Admin").Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = "Admin";
                    IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
                }
            }
        }
    }


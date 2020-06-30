using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Database.Initializer
{
    public  class Seed
    {
        public static void SeedData(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var roles = new List<Role>()
           {
               new Role(){Name="Admin"},
               new Role(){Name="User"}
           };

                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).GetAwaiter().GetResult();
                }

                //add some default user

                var users = new List<User>()
            {
                new User(){Email="sachindewan12@gmail.com",PhoneNumber= "8800255672",UserName="sachinkumar@123"},
                new User(){Email="sachindewan12@gmail.com",PhoneNumber= "8800255672",UserName="Riyan"},
                new User(){Email="sachindewan12@gmail.com",PhoneNumber= "8800255672",UserName="Gaurav"},
                new User(){Email="sachindewan12@gmail.com",PhoneNumber= "8800255672",UserName="Sanjay"}
            };
                foreach (var user in users)
                {
                    var result = userManager.CreateAsync(user, "user@123").GetAwaiter().GetResult();
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "User").GetAwaiter().GetResult();
                    }
                }

                var adminUser = new User() { Email = "admin@gmail.com", PhoneNumber = "8800255672", UserName = "admin" };
                var adminResult = userManager.CreateAsync(adminUser, "admin@123").GetAwaiter().GetResult();
                if (adminResult.Succeeded)
                {
                    var admin = userManager.FindByNameAsync(adminUser.UserName).GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(admin, "Admin").GetAwaiter().GetResult();
                }
            }
        }
    }
}

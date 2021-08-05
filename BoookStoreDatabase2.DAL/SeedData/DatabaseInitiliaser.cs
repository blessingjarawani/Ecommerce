using BoookStoreDatabase2.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoookStoreDatabase2.DAL.SeedData
{
    public class DatabaseInitiliaser
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {

            var users = new List<ApplicationUser>
            {
               new ApplicationUser
               {
                 Email = "jahb@gmail.com",
                 UserName = "Jahb",
                 User = new Administrator{ FirstName="Blessing", LastName ="Rita", IsActive =true, DOB = DateTime.Parse("1994-01-01"), DateHired = DateTime.Parse("2000-01-01")}
               },
                new ApplicationUser {
                 Email = "bj@gmail.com",
                 UserName = "Blessing",
                 User = new Administrator{FirstName="Prosper", LastName ="Ruth", IsActive =true, DOB = DateTime.Parse("1994-01-01"), DateHired = DateTime.Parse("2000-01-01")}
               },
               new ApplicationUser   {
                 Email = "luck@gmail.com",
                 UserName = "Luck",
                  
                 Customer = new Customer{FirstName="LuckMore", LastName ="Tom", IsActive =true, DOB = DateTime.Parse("1994-01-01")}
               },
                new ApplicationUser   {
                 Email = "josphat@gmail.com",
                 UserName = "Josphat",
                 
                 Customer = new Customer{FirstName="John", LastName ="Roster", IsActive =true, DOB = DateTime.Parse("1994-01-01")}
               },
                new ApplicationUser   {
                 Email = "bless@gmail.com",
                 UserName = "Bless",
                
                 Customer = new Customer{FirstName="Bless", LastName ="Bless", IsActive =true, DOB = DateTime.Parse("1994-01-01")}
               },
            };
            users.ForEach(x =>
           {
               if (userManager.FindByNameAsync(x.Email).Result == null)
               {
                   var result = userManager.CreateAsync(x, "password").Result;
                   if (result.Succeeded)
                   {
                       var role = x.Customer != null ? "Customer" : "Employee";
                       userManager.AddToRoleAsync(x, role).Wait();
                   }

               }

           });

        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Customer").Result)
            {
                var role = new IdentityRole();
                role.Name = "Customer";
                role.NormalizedName = "Customer";
                var roleResult = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Employee",
                    NormalizedName = "Employee operations.",
                };
                var roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}


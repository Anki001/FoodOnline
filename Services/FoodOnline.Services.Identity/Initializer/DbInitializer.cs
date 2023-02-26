using FoodOnline.Services.Identity.Common;
using FoodOnline.Services.Identity.DBContexts;
using FoodOnline.Services.Identity.Initializer.Interfaces;
using FoodOnline.Services.Identity.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Security.Claims;

namespace FoodOnline.Services.Identity.Initializer
{
    public class DbInitializer : IDbInitializer
    {        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {            
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            if (_roleManager.FindByNameAsync(Constants.Admin).Result != null)
                return;

            _roleManager.CreateAsync(new IdentityRole(Constants.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Constants.Customer)).GetAwaiter().GetResult();

            ApplicationUser adminUser = new ApplicationUser
            {
                UserName = "ank.gawande@gmail.com",
                Email = "ank.gawande@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
                FirstName = "Ankush",
                LastName = "Admin"
            };

            _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(adminUser, Constants.Admin).GetAwaiter().GetResult();

            var adminClaims = _userManager.AddClaimsAsync(adminUser, new Claim[] {
                new Claim(JwtClaimTypes.Name, adminUser.FirstName +" "+ adminUser.LastName),
                new Claim(JwtClaimTypes.GivenName, adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
                new Claim(JwtClaimTypes.Role, Constants.Admin),
            }).Result;

            ApplicationUser customerUser = new ApplicationUser
            {
                UserName = "gawande.ankush@ymail.com",
                Email = "gawande.ankush@ymail.com",
                EmailConfirmed = true,
                PhoneNumber = "0987654321",
                FirstName = "Arush",
                LastName = "Customer"
            };

            _userManager.CreateAsync(customerUser, "Customer123*").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customerUser, Constants.Customer).GetAwaiter().GetResult();

            var customerClaims = _userManager.AddClaimsAsync(customerUser, new Claim[] {
                new Claim(JwtClaimTypes.Name, customerUser.FirstName +" "+ customerUser.LastName),
                new Claim(JwtClaimTypes.GivenName, customerUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
                new Claim(JwtClaimTypes.Role, Constants.Customer),
            }).Result;
        }
    }
}

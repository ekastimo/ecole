using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Auth.Models;
using App.Areas.Auth.Services.Account;
using App.Areas.Auth.ViewModels.Account;
using App.Areas.Crm.Enums;
using App.Areas.Crm.Services;
using App.Areas.Events.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace App.Data
{
    public class Seed
    {
        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Startup>();
                try
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();
                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var isSeeded = await roleManager.RoleExistsAsync(SystemRoles.Super);
                    if (isSeeded)
                    {
                        return;
                    }

                    var roles = new List<string> { SystemRoles.Super, SystemRoles.Admin, SystemRoles.User };
                    foreach (var role in roles)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }

                    var userView = new RegisterViewModel
                    {
                        FirstName = "Super",
                        OtherNames = "Admin",
                        Email = "ekastimo@gmail.com",
                        Phone = "+256700106164",
                        DateOfBirth = new DateTime(1990, 12, 20),
                        Gender = Gender.Male,
                        Password = "Xpass@123",
                        ConfirmPassword = "Xpass@123"
                    };
                    var user = await accountService.Register(userView);

                    var result = await userMgr.AddToRolesAsync(user, roles);
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    var contactService = scope.ServiceProvider.GetRequiredService<IContactService>();
                    var data = Faker.FakeContacts();
                    foreach (var contact in data)
                    {
                        logger.LogInformation(JsonConvert.SerializeObject(contact));
                        await contactService.CreateAsync(contact);
                    }
                    
                    logger.LogWarning( $"Added {data.Count} contacts");

                    var eventService = scope.ServiceProvider.GetRequiredService<IEventService>();
                    var eventsData = Faker.FakeEvents();
                    foreach (var evnet in eventsData)
                    {
                        logger.LogInformation(JsonConvert.SerializeObject(evnet));
                        await eventService.CreateAsync(evnet);
                    }

                    logger.LogWarning($"Added {eventsData.Count} events");

                }
                catch (Exception e)
                {
                    logger.LogError(e,"error in seeding data");
                    throw;
                }
            }
        }
    }
}
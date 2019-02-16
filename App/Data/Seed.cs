using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Areas.Auth.Models;
using App.Areas.Auth.Services.Account;
using App.Areas.Auth.ViewModels.Account;
using App.Areas.Crm.Enums;
using App.Areas.Crm.Services;
using App.Areas.Events.Services.Event;
using App.Areas.Events.Services.Item;
using Microsoft.AspNetCore.Identity;
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
                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                    var isSeeded = await roleManager.RoleExistsAsync(SystemRoles.Super);
                    if (isSeeded)
                    {
                        return;
                    }

                    var roles = new List<string> {SystemRoles.Super, SystemRoles.Admin, SystemRoles.User};
                    foreach (var role in roles)
                    {
                        await roleManager.CreateAsync(new ApplicationRole(role));
                    }

                    var userView = new RegisterViewModel
                    {
                        FirstName = "Super",
                        LastName = "Admin",
                        Email = "ekastimo@gmail.com",
                        Phone = "+256700106164",
                        DateOfBirth = new DateTime(1990, 12, 20),
                        Gender = Gender.Male,
                        Password = "Xpass@123",
                        ConfirmPassword = "Xpass@123"
                    };
                    var user = await accountService.Register(userView);
                    var adminId = user.ContactId;
                    var result = await userMgr.AddToRolesAsync(user, roles);
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    var contactService = scope.ServiceProvider.GetRequiredService<IContactService>();
                    var fakeContacts = FakeData.FakeContacts();
                    foreach (var contact in fakeContacts)
                    {
                        logger.LogInformation(JsonConvert.SerializeObject(contact));
                        await contactService.CreateAsync(contact);
                    }

                    logger.LogWarning($"Added {fakeContacts.Count} contacts");

                    var eventService = scope.ServiceProvider.GetRequiredService<IEventService>();
                    var itemService = scope.ServiceProvider.GetRequiredService<IItemService>();
                    var eventsData = FakeData.FakeEvents(adminId);
                    foreach (var eventView in eventsData)
                    {
                        var saved = await eventService.CreateAsync(eventView);
                        var items = FakeData.FakeEventItems(adminId, saved.Id, 12);
                        foreach (var item in items)
                        {
                            await itemService.CreateAsync(item);
                        }
                    }

                    logger.LogWarning($"Added {eventsData.Count} events");
                }
                catch (Exception e)
                {
                    logger.LogError(e, "error in seeding data");
                    throw;
                }
            }
        }
    }
}
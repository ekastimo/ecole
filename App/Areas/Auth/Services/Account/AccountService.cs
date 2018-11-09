using System.Linq;
using System.Threading.Tasks;
using App.Areas.Auth.Models;
using App.Areas.Auth.ViewModels.Account;
using App.Areas.Auth.ViewModels.Manage;
using App.Areas.Crm.Services;
using App.Areas.Crm.ViewModels;
using Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace App.Areas.Auth.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountService> _logger;
        private readonly IContactService _contactService;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ILogger<AccountService> logger,
            IContactService contactService
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
            _contactService = contactService;
        }

        public async Task<ApplicationUser> Register([FromBody] RegisterViewModel model)
        {
            _logger.LogInformation($"creating new user email:{model.Email}");
            var contactModel = new NewPersonViewModel
            {
                FirstName = model.FirstName,
                OtherNames = model.OtherNames,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Email = model.Email,
                Phone = model.Phone
            };
            var contactExists = await _contactService.ContactExistsByEmailAsync(model.Email);
            if (contactExists)
            {
                _logger.LogInformation($"registartion.failed duplicate email: ${model.Email}");
                throw new DuplicateEntityException($"Email: {model.Email} already has an account");
            }

            var contact = await _contactService.CreateAsync(contactModel);
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                ContactId = contact.Id
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(it => $"{it.Code}: {it.Description}").ToArray();
                _logger.LogError($"user-creation.failed {string.Join(",", errors)}");
                throw new ClientFriendlyException("Oops, we were unable to register you, please try again");
            }
            return user;
        }

        public async Task<object> AssignRoles(AssignRolesViewModel model)
        {
            var appUser = _userManager.Users.SingleOrDefault(r => r.UserName == model.Username);
            if (appUser == null)
            {
                throw new NotFoundException($"invalid user:${model.Username}");
            }

            var result = await _userManager.AddToRolesAsync(appUser, model.Roles);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(it => $"{it.Code}: {it.Description}").ToArray();
                _logger.LogError($"user-creation.failed {string.Join(",", errors)}");
                throw new ClientFriendlyException("Oops, operation failed, please try again");
            }

            return new
            {
                Status = true,
                Message = "Operation successful"
            };
        }
    }
}
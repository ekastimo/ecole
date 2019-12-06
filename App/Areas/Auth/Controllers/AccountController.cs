using App.Areas.Auth.Models;
using App.Areas.Auth.Services.Account;
using App.Areas.Auth.ViewModels;
using App.Areas.Auth.ViewModels.Account;
using App.Areas.Crm.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using App.Areas.Crm.Repositories;
using App.Data;
using MongoDB.Driver;

namespace App.Areas.Auth.Controllers
{
    [Route("api/auth")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public AccountController(IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IAccountService accountService,
            IContactRepository contactRepository,
            IMapper mapper, ApplicationDbContext context

        )
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _accountService = accountService;
            _contactRepository = contactRepository;
            _mapper = mapper;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<object> Login([FromBody] LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                return await GenerateJwtToken(model.Email, appUser);
            }

            throw new NotAuthorizedException("Oops, invalid username / password");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<object> Register([FromBody] RegisterViewModel model)
        {
            var user = await _accountService.Register(model);
            return await GenerateJwtToken(model.Email, user);
        }

        private async Task<object> GenerateJwtToken(string email, ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var contact = await _contactRepository.GetByIdAsync(user.ContactId);
            var person = _mapper.Map<PersonViewModel>(contact.Person);
            var fullName = $"{person.FirstName} {person.MiddleName} {person.LastName}"
                .Replace("  ", " ").Trim();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("id", user.Id),
                new Claim("contactId", contact.Id.ToString()),
                new Claim("fullName",  fullName)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(_configuration["JwtIssuer"],_configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new
            {
                Token = tokenString,
                User = new UserViewModel
                {
                    Id = user.Id,
                    ContactId = user.ContactId,
                    Email = user.Email,
                    Username = user.UserName,
                    Roles = roles,
                    FullName = fullName
                }
            };
        }

        [Authorize(Roles = AuthConstants.Admin)]
        [HttpGet("users")]
        public List<UserViewModel> Get()
        {
            var result = (from u in _userManager.Users.AsQueryable()
                join c in _context.Contacts.AsQueryable() on u.ContactId equals c.Id
                select new UserViewModel
                {
                    Id = u.Id,
                    Email = u.Email,
                    Username = u.UserName,
                    ContactId = u.ContactId,
                    FullName = $"{c.Person.FirstName} {c.Person.LastName}",
                    Avatar = c.Person.Avatar,
                    Roles = u.Roles
                }).ToList();
            return result;
        }


        [Authorize(Roles = AuthConstants.Admin)]
        [HttpGet("users/{id}")]
        public async Task<UserViewModel> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"Invalid user: {id}");
            var roles = await _userManager.GetRolesAsync(user);
            return new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Roles = roles
            };
        }

        [HttpGet("profile")]
        public async Task<UserViewModel> GetById()
        {
            var (userId, userClaims) = _httpContextAccessor.GetUser();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Invalid user: {userId}");
            var roles = await _userManager.GetRolesAsync(user);
            var name = userClaims["fullName"];
            return new UserViewModel
            {
                Id = user.Id,
                ContactId = user.ContactId,
                Email = user.Email,
                Username = user.UserName,
                Roles = roles,
                FullName = name
            };

        }
    }
}